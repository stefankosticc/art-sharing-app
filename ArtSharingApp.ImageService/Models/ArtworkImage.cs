namespace ArtSharingApp.ImageService.Models;

public class ArtworkImage
{
    public Guid Id { get; private set; }
    public int ArtworkRefId { get; private set; }
    public byte[] ImageData { get; private set; }
    public string ContentType { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ArtworkImage() { }

    public static ArtworkImage Create(int artworkRefId, byte[] imageData, string contentType)
        => new ArtworkImage
        {
            Id = Guid.NewGuid(),
            ArtworkRefId = artworkRefId,
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
