namespace ArtSharingApp.Backend.Utils;

public static class ImagePaths
{
    public static string Artwork(string imageId) => $"/images/artworks/{imageId}";

    public static string? UserProfilePhoto(string? photoId)
        => photoId != null ? $"/images/users/profile-photo/{photoId}" : null;
}