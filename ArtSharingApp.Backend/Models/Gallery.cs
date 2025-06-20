namespace ArtSharingApp.Backend.Models;

public class Gallery
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }
    public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
}

