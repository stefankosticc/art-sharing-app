using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class OfferRepository : GenericRepository<Offer>, IOfferRepository
{
    public OfferRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<decimal> GetMaxOfferAmountAsync(int auctionId)
    {
        var maxOfferAmount = await _dbSet
            .Where(o => o.AuctionId == auctionId)
            .MaxAsync(o => (decimal?)o.Amount);
        return maxOfferAmount ?? 0;
    }

    public async Task<IEnumerable<Offer>> GetOffersByAuctionIdAsync(int auctionId)
    {
        return await _dbSet
            .Where(o => o.AuctionId == auctionId)
            .Include(o => o.User)
            .OrderByDescending(o => o.Timestamp)
            .ToListAsync();
    }

    public void UpdateOfferStatus(Offer offer)
    {
        _context.Attach(offer);
        _context.Entry(offer).Property(o => o.Status).IsModified = true;
    }

    public async Task<int> GetOfferCountByAuctionIdAsync(int auctionId)
    {
        return await _dbSet.Where(o => o.AuctionId == auctionId).CountAsync();
    }
}