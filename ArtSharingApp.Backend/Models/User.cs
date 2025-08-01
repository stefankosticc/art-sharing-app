using Microsoft.AspNetCore.Identity;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a user in the application.
/// Inherits from <see cref="IdentityUser{TKey}"/> with integer key.
/// </summary>
public class User : IdentityUser<int>
{
    /// <summary>
    /// Name of the user.
    /// <remarks>
    /// This property is used to store the full name of the user.
    /// </remarks>
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Biography of the user.
    /// </summary>
    public string? Biography { get; set; }

    /// <summary>
    /// Unique identifier for the role assigned to the user.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Navigation property to the role assigned to the user.
    /// </summary>
    public Role? Role { get; set; }

    /// <summary>
    /// Refresh token used for re-authentication.
    /// <remarks>
    /// This token is used to obtain a new access token without requiring the user to log in again.
    /// </remarks>
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Date and time when the refresh token expires.
    /// <remarks>
    /// This property is used to determine when the refresh token is no longer valid.
    /// </remarks>
    /// </summary>
    public DateTime? RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// User's profile photo in byte array format.
    /// </summary>
    public byte[]? ProfilePhoto { get; set; }

    /// <summary>
    /// Content type of the profile photo.
    /// <remarks>
    /// This is used to determine how the profile photo should be displayed in the UI.
    /// </remarks>
    /// <example>
    /// "image/jpeg"
    /// </example>
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Collection of artworks created by the user.
    /// </summary>
    public ICollection<Artwork> CreatedArtworks { get; set; } = new List<Artwork>();

    /// <summary>
    /// Collection of artworks posted by the user.
    /// </summary>
    public ICollection<Artwork> PostedArtworks { get; set; } = new List<Artwork>();

    /// <summary>
    /// Collection of favorite artworks liked by the user.
    /// </summary>
    public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();

    /// <summary>
    /// Collection of users that follow this user
    /// </summary>
    public ICollection<Followers> Followers { get; set; } = new List<Followers>();

    /// <summary>
    /// Collection of users that this user is following
    /// </summary>
    public ICollection<Followers> Following { get; set; } = new List<Followers>();

    /// <summary>
    /// Collection of notifications sent to the user.
    /// </summary>
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    /// <summary>
    /// Collection of offers made by the user on auctions.
    /// </summary>
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();

    /// <summary>
    /// Collection of chat messages sent by the user.
    /// </summary>
    public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();

    /// <summary>
    /// Collection of chat messages that the user has received.
    /// </summary>
    public ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();
}