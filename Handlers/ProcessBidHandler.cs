using MediatR;
using BidFunctionApp.Requests;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using BidFunctionApp.Models;

namespace BidFunctionApp.Handlers
{
    public class ProcessBidHandler : IRequestHandler<ProcessBidRequest, string>
    {
        private readonly IConnectionFactory _connectionFactory;

        public ProcessBidHandler(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<string> Handle(ProcessBidRequest request, CancellationToken cancellationToken)
        {
            var referenceId = Guid.NewGuid().ToString();
            request.BidData.ReferenceId = referenceId;

            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var message = JsonSerializer.Serialize(request.BidData);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: ConstantValues.BidsQueueName, body: body, cancellationToken: cancellationToken);           
            return referenceId;
        }
    }
}
