using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IArtworkService
{
    Task<IEnumerable<ArtworkResponseDTO>> GetAllAsync();
    Task<ArtworkResponseDTO?> GetByIdAsync(int id);
    Task AddAsync(ArtworkRequestDTO artworkDto);
    Task UpdateAsync(int id, ArtworkRequestDTO artworkDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<ArtworkResponseDTO>?> SearchByTitle(string title);
    Task ChangeVisibilityAsync(int id, bool isPrivate);
    Task PutOnSaleAsync(int id, int loggedInUserId, PutArtworkOnSaleDTO request);
    Task RemoveFromSaleAsync(int id, int loggedInUserId);
    Task TransferToUserAsync(int artworkId, int fromUserId, int toUserId);
    Task<IEnumerable<ArtworkPreviewDTO>?>  GetMyArtworksAsync(int loggedInUserId);
}
