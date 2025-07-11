using ArtSharingApp.Backend.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IArtworkService
{
    Task<IEnumerable<ArtworkResponseDTO>> GetAllAsync();
    Task<ArtworkResponseDTO?> GetByIdAsync(int id, int loggedInUserId);
    Task AddAsync(ArtworkRequestDTO artworkDto, IFormFile artworkImage);
    Task UpdateAsync(int id, ArtworkRequestDTO artworkDto, IFormFile? artworkImage);
    Task DeleteAsync(int id);
    Task<IEnumerable<ArtworkSearchResponseDTO>?> SearchByTitle(string title);
    Task ChangeVisibilityAsync(int id, bool isPrivate);
    Task PutOnSaleAsync(int id, int loggedInUserId, PutArtworkOnSaleDTO request);
    Task RemoveFromSaleAsync(int id, int loggedInUserId);
    Task TransferToUserAsync(int artworkId, int fromUserId, int toUserId);
    Task<IEnumerable<ArtworkPreviewDTO>?>  GetMyArtworksAsync(int loggedInUserId);
    Task<(byte[] Image, string ContentType)> GetArtworkImageAsync(int id);
    Task<string?> ExtractColorAsync(IFormFile image);
}
