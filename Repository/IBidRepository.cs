using BidFunctionApp.Models;

namespace BidFunctionApp.Repository
{
    public interface IBidRepository
    {
        Task<int> AddBidAsync(Bid bid);
        Task<Bid?> GetBidByIdAsync(int id);
        Task<IEnumerable<Bid>> GetAllBidsAsync();
    }
}
