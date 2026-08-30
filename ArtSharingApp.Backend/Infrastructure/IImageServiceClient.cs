using ArtSharingApp.Backend.DTO.ImageService;
using Refit;

namespace ArtSharingApp.Backend.Infrastructure;

public interface IImageServiceClient
{
    [Multipart]
    [Post("/images/artworks?artworkRefId={artworkRefId}")]
    Task<ArtworkImageDTO> UploadArtworkImageAsync(int artworkRefId, [AliasAs("image")] StreamPart file);

    [Multipart]
    [Put("/images/artworks/{imageId}")]
    Task<ArtworkImageDTO> ReplaceArtworkImageAsync(string imageId, [AliasAs("image")] StreamPart file);

    [Delete("/images/artworks/{imageId}")]
    Task DeleteArtworkImageAsync(string imageId);

    [Multipart]
    [Post("/images/users/profile-photo?userRefId={userRefId}")]
    Task<UserProfilePhotoDTO> UploadUserPhotoAsync(int userRefId, [AliasAs("image")] StreamPart file);

    [Multipart]
    [Put("/images/users/profile-photo/{imageId}")]
    Task<UserProfilePhotoDTO> ReplaceUserPhotoAsync(string imageId, [AliasAs("image")] StreamPart file);

    [Delete("/images/users/profile-photo/{imageId}")]
    Task DeleteUserPhotoAsync(string imageId);
}