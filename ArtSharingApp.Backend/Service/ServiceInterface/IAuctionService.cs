using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IAuctionService
{
    Task StartAuctionAsync(int artworkId, int userId, AuctionStartDTO request);
    Task MakeAnOfferAsync(int auctionId, int userId, OfferRequestDTO request);
    Task<IEnumerable<OfferResponseDTO>?> GetOffersAsync(int auctionId, int userId);
    Task<decimal?> GetMaxOfferAsync(int auctionId);
    Task AcceptOfferAsync(int offerId, int userId);
    Task RejectOfferAsync(int offerId, int userId);
    Task WithdrawOfferAsync(int offerId, int userId);
    Task<AuctionResponseDTO?> GetActiveAuctionAsync(int artworkId);
    Task UpdateAuctionEndTimeAsync(int auctionId, int userId, AuctionUpdateEndDTO request);
    Task<IEnumerable<HighStakesAuctionDTO>?> GetHighStakesAuctionsAsync(int count);
}