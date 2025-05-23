using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IAuctionRepository : IGenericRepository<Auction>
{
    Task<bool> IsAuctionScheduledAsync(int artworkId, DateTime requestStartTime, DateTime requestEndTime);
}