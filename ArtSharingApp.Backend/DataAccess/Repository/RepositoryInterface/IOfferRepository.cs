using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IOfferRepository : IGenericRepository<Offer>
{
    Task<decimal> GetMaxOfferAmountAsync(int auctionId);
    Task<IEnumerable<Offer>> GetOffersByAuctionIdAsync(int auctionId);
    void UpdateOfferStatus(Offer offer);
}