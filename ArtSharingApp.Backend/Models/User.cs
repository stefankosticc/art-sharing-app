using Microsoft.AspNetCore.Identity;

namespace ArtSharingApp.Backend.Models;

public class User : IdentityUser<int>
{ 
    public string Name { get; set; }
    public string? Biography { get; set; }
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public ICollection<Artwork> CreatedArtworks { get; set; } = new List<Artwork>();
    public ICollection<Artwork> PostedArtworks { get; set; } = new List<Artwork>();
    public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();
}
