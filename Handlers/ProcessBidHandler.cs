using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BidFunctionApp.Requests;
using BidFunctionApp.Repository;

namespace BidFunctionApp.Handlers
{
    public class ProcessBidHandler : IRequestHandler<ProcessBidRequest, string>
    {
        private IBidRepository _bidRepository;

        public ProcessBidHandler(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<string> Handle(ProcessBidRequest request, CancellationToken cancellationToken)
        {

            var id = await _bidRepository.AddBidAsync(new Models.Bid()
            {
                Amount = 10,
                BidderName = "test",
                BidTime = DateTime.UtcNow,
            });

            // Handle the bid processing logic
            return id.ToString();
        }
    }
}
