using ArtSharingApp.ImageService.DTO;

namespace ArtSharingApp.ImageService.Services;

public interface IArtworkImageService
{
    Task<ArtworkImageDTO> UploadAsync(int artworkRefId, IFormFile file);
    Task<(byte[] Data, string ContentType)> GetByIdAsync(Guid imageId);
    Task<ArtworkImageDTO> ReplaceAsync(Guid imageId, IFormFile file);
    Task DeleteAsync(Guid imageId);
}