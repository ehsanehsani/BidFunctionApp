using BidFunctionApp.Models;
using BidFunctionApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class BidRepository : IBidRepository
{
    private readonly AppDbContext _context;

    public BidRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddBidAsync(Bid bid)
    {
        await _context.Bids.AddAsync(bid);
        await _context.SaveChangesAsync();
        return bid.Id;
    }

    public async Task<Bid?> GetBidByIdAsync(int id)
    {
        return await _context.Bids.FindAsync(id);
    }

    public async Task<IEnumerable<Bid>> GetAllBidsAsync()
    {
        return await _context.Bids.ToListAsync();
    }
}
