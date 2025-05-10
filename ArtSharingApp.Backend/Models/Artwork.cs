namespace ArtSharingApp.Backend.Models;

public class Artwork
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Story { get; set; }
    public string Image { get; set; }
    public DateOnly Date { get; set; }
    public string TipsAndTricks { get; set; }
    public int? CityId { get; set; }
    public City? City { get; set; }
    public int? GalleryId { get; set; }
    public Gallery? Gallery { get; set; }
    public int CreatedByArtistId { get; set; }
    public User CreatedByArtist { get; set; }
    public int PostedByUserId { get; set; }
    public User PostedByUser { get; set; }
    public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();
}
