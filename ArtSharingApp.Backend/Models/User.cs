namespace ArtSharingApp.Backend.Models;

public class User
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Biography { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } // required (1,1)
    public ICollection<Artwork> CreatedArtworks { get; set; } = new List<Artwork>();
    public ICollection<Artwork> PostedArtworks { get; set; } = new List<Artwork>();
    public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();
}
