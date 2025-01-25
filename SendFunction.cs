using System.ComponentModel.DataAnnotations;
using BidFunctionApp.Models;
using BidFunctionApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BidFunctionApp
{
    public class SendFunction
    {
        private readonly ILogger<SendFunction> _logger;
        private readonly IMediator _mediator;

        public SendFunction(ILogger<SendFunction> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Function("SendFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {

                // Read the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                // Deserialize the request body to a Bid object
                var bid = System.Text.Json.JsonSerializer.Deserialize<Bid>(requestBody, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Check if the bid is null
                if (bid == null)
                {
                    throw new ValidationException("The bid object is valid. Please provide a valid bid payload.");
                }

                var request = new ProcessBidRequest(bid);
                var referenceId = await _mediator.Send(request);

                _logger.LogInformation("C# HTTP trigger function processed a request.");
                return new CreatedResult($"/api/bids/{referenceId}", new { Message = "The bid has been successfully generated." });
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
