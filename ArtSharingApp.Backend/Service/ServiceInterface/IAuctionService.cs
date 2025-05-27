using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IAuctionService
{
    Task StartAuctionAsync(int artworkId, int userId, AuctionStartDTO request);
    Task MakeAnOfferAsync(int auctionId, int userId, OfferRequestDTO request);
    Task<IEnumerable<OfferResponseDTO>?> GetOffersAsync(int auctionId, int userId);
    Task<decimal?> GetMaxOfferAsync(int auctionId);
    Task AcceptOfferAsync(int offerId, int userId);
    Task WithdrawOfferAsync(int offerId, int userId);
}