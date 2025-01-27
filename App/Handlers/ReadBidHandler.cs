using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BidFunctionApp.Requests;
using BidFunctionApp.Repository;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using BidFunctionApp.Models;

namespace BidFunctionApp.Handlers
{
    public class ReadBidHandler : IRequestHandler<ReadBidRequest, Bid>
    {
        private IBidRepository _bidRepository;

        public ReadBidHandler(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<Bid> Handle(ReadBidRequest request, CancellationToken cancellationToken)
        {
            var referenceId = request.ReferenceId;
            var result = await _bidRepository.GetBidByReferenceIdAsync(referenceId);
            return result;
        }
    }
}
