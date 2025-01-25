using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BidFunctionApp.Requests;
using BidFunctionApp.Repository;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace BidFunctionApp.Handlers
{
    public class ProcessBidHandler : IRequestHandler<ProcessBidRequest, string>
    {
        public async Task<string> Handle(ProcessBidRequest request, CancellationToken cancellationToken)
        {
            var referenceId = Guid.NewGuid().ToString();
            request.BidData.ReferenceId = referenceId;

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var message = JsonSerializer.Serialize(request.BidData);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "bids_queue", body: body, cancellationToken: cancellationToken);           
            return referenceId;
        }
    }
}
