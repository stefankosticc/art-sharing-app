using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class AuctionRepository : GenericRepository<Auction>, IAuctionRepository
{
    public AuctionRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<bool> IsAuctionScheduledAsync(int artworkId, DateTime requestStartTime, DateTime requestEndTime)
    {
        return await _dbSet.AnyAsync(a => a.ArtworkId == artworkId && 
                                 a.StartTime < requestEndTime && 
                                 a.EndTime > requestStartTime);
    }

    public async Task<bool> HasFutureAuctionScheduledAsync(int artworkId, DateTime fromTime)
    {
        return await _dbSet.AnyAsync(a => a.ArtworkId == artworkId && a.EndTime >= fromTime);
    }

    public async Task<Auction?> GetActiveAuctionByArtworkIdAsync(int artworkId, DateTime now)
    {
        return await _dbSet
            .Where(a => a.ArtworkId == artworkId && a.StartTime <= now && a.EndTime >= now)
            .FirstOrDefaultAsync();
    }
}