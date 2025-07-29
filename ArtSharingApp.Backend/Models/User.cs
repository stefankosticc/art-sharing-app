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
    public byte[]? ProfilePhoto { get; set; }
    public string? ContentType { get; set; }
    public ICollection<Artwork> CreatedArtworks { get; set; } = new List<Artwork>();
    public ICollection<Artwork> PostedArtworks { get; set; } = new List<Artwork>();
    public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();
    public ICollection<Followers> Followers { get; set; } = new List<Followers>();
    public ICollection<Followers> Following { get; set; } = new List<Followers>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
    public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();
}