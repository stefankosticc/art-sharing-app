namespace ArtSharingApp.Backend.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();
    public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
}

