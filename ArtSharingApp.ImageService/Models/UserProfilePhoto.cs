namespace ArtSharingApp.ImageService.Models;

public class UserProfilePhoto
{
    public Guid Id { get; private set; }
    public int UserRefId { get; private set; }
    public byte[] ImageData { get; private set; }
    public string ContentType { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private UserProfilePhoto()
    {
    }

    public static UserProfilePhoto Create(int userRefId, byte[] imageData, string contentType)
        => new UserProfilePhoto
        {
            Id = Guid.NewGuid(),
            UserRefId = userRefId,
            ImageData = imageData,
            ContentType = contentType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

    public void Update(byte[] imageData, string contentType)
    {
        ImageData = imageData;
        ContentType = contentType;
        UpdatedAt = DateTime.UtcNow;
    }
}