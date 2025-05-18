namespace ArtSharingApp.Backend.DTO;

public class ArtworkResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Story { get; set; }
    public string Image { get; set; }
    public DateOnly Date { get; set; }
    public string TipsAndTricks { get; set; }
    public bool IsPrivate { get; set; }
    public int CreatedByArtistId { get; set; }
    public string CreatedByArtistName { get; set; }
    public int PostedByUserId { get; set; }
    public string PostedByUserName { get; set; }
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public int? GalleryId { get; set; }
    public string? GalleryName { get; set; }
}

public class ArtworkRequestDTO
{
    public string Title { get; set; }
    public string Story { get; set; }
    public string Image { get; set; }
    public DateOnly Date { get; set; }
    public string TipsAndTricks { get; set; }
    public bool IsPrivate { get; set; }
    public int CreatedByArtistId { get; set; }
    public int PostedByUserId { get; set; }
    public int? CityId { get; set; }
    public int? GalleryId { get; set; }
}

public class ChangeArtworkVisibilityDTO
{
    public bool IsPrivate { get; set; }
}
