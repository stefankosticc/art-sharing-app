namespace ArtSharingApp.ImageService.DTO;

public class ArtworkImageDTO
{
    public Guid ImageId { get; set; }
    public string Url { get; set; }
    public string? DominantColor { get; set; }
}
