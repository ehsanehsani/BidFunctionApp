using System.ComponentModel.DataAnnotations;
using BidFunctionApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BidFunctionApp
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IMediator _mediator;

        public Function1(ILogger<Function1> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //TODO: REMOVE GET
        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            try
            {
                var request = new ProcessBidRequest("test");
                var newBidId = await _mediator.Send(request);

                _logger.LogInformation("C# HTTP trigger function processed a request.");
                return new CreatedResult($"/api/bids/{newBidId}", new { Message = "The bid has been successfully generated." });
            }
            catch (DbUpdateException dbEx)
            {
                // Log database-specific errors
                _logger.LogError(dbEx, "A database error occurred while processing the bid.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // 500 Internal Server Error
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
