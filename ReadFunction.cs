using System.ComponentModel.DataAnnotations;
using BidFunctionApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BidFunctionApp
{
    public class ReadFunction
    {
        private readonly ILogger<ReadFunction> _logger;
        private readonly IMediator _mediator;

        public ReadFunction(ILogger<ReadFunction> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Function("ReadFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            try
            {
                // Retrieve the GUID from the query parameters safely
                if (!req.Query.TryGetValue("guid", out var guidString) || string.IsNullOrEmpty(guidString))
                {
                    return new BadRequestObjectResult(new { Error = "The 'guid' query parameter is required." });
                }

                // Validate that the provided string is a valid GUID
                if (!Guid.TryParse(guidString, out Guid guid))
                {
                    return new BadRequestObjectResult(new { Error = "The provided 'guid' is not a valid GUID." });
                }

                // Validate that the GUID is provided
                if (string.IsNullOrEmpty(guidString))
                {
                    return new BadRequestObjectResult(new { Error = "The 'guid' query parameter is required." });
                }

                var request = new ReadBidRequest(guid);
                var bidResult = await _mediator.Send(request);

                if (bidResult == null)
                {
                    return new NotFoundObjectResult("Can't find the bid.");
                }

                _logger.LogInformation("C# HTTP trigger function processed a request.");
                return new OkObjectResult(bidResult);
            }
            catch (ValidationException valEx)
            {
                // Log validation errors and return 400 Bad Request
                _logger.LogWarning(valEx, "Validation error: {Message}", valEx.Message);
                return new BadRequestObjectResult(new { Error = valEx.Message }); // 400 Bad Request
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                _logger.LogError(ex, "An unexpected error occurred.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // 500 Internal Server Error
            }
        }
    }
}
