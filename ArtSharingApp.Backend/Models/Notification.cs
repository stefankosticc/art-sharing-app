using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a notification sent to a user
/// </summary>
public class Notification
{
    /// <summary>
    /// Unique identifier for the notification
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Text content of the notification
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Date and time when the notification was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Status of the notification. See <see cref="NotificationStatus"/>.
    /// </summary>
    public NotificationStatus Status { get; set; }

    /// <summary>
    /// Unique identifier for the user who receives the notification
    /// </summary>
    public int RecipientId { get; set; }

    /// <summary>
    /// Navigation property for the user who receives the notification
    /// </summary>
    public User Recipient { get; set; }
}