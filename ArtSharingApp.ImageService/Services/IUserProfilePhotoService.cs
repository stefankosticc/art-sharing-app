using ArtSharingApp.ImageService.DTO;

namespace ArtSharingApp.ImageService.Services;

public interface IUserProfilePhotoService
{
    Task<UserProfilePhotoDTO> UploadAsync(int userRefId, IFormFile file);
    Task<(byte[] Data, string ContentType)> GetByIdAsync(Guid photoId);
    Task<UserProfilePhotoDTO> ReplaceAsync(Guid photoId, IFormFile file);
    Task DeleteAsync(Guid photoId);
}
