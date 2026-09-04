using ArtSharingApp.Backend.DTO.ImageService;
using ArtSharingApp.Backend.Infrastructure;
using Refit;

namespace ArtSharingApp.Tests.IntegrationTests;

public class FakeImageServiceClient : IImageServiceClient
{
    public Task<ArtworkImageDTO> UploadArtworkImageAsync(int artworkRefId, StreamPart file)
        => Task.FromResult(new ArtworkImageDTO { ImageId = Guid.NewGuid(), Url = "/images/artworks/fake" });

    public Task<ArtworkImageDTO> ReplaceArtworkImageAsync(string imageId, StreamPart file)
        => Task.FromResult(new ArtworkImageDTO { ImageId = Guid.NewGuid(), Url = $"/images/artworks/{imageId}" });

    public Task DeleteArtworkImageAsync(string imageId) => Task.CompletedTask;

    public Task<UserProfilePhotoDTO> UploadUserPhotoAsync(int userRefId, StreamPart file)
        => Task.FromResult(new UserProfilePhotoDTO { ImageId = Guid.NewGuid(), Url = "/images/users/profile-photo/fake" });

    public Task<UserProfilePhotoDTO> ReplaceUserPhotoAsync(string imageId, StreamPart file)
        => Task.FromResult(new UserProfilePhotoDTO { ImageId = Guid.NewGuid(), Url = $"/images/users/profile-photo/{imageId}" });

    public Task DeleteUserPhotoAsync(string imageId) => Task.CompletedTask;
}
