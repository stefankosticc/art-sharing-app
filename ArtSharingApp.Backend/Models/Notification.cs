using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

public class Notification
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public NotificationStatus Status { get; set; }
    public int RecipientId { get; set; }
    public User Recipient { get; set; }
}