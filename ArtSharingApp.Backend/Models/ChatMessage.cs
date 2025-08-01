namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents a chat message between users
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Unique identifier for the chat message
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique identifier for the sender of the message
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// Navigation property to the user who sent the message
    /// </summary>
    public User Sender { get; set; }

    /// <summary>
    /// Unique identifier for the receiver of the message
    /// </summary>
    public int ReceiverId { get; set; }

    /// <summary>
    /// Navigation property to the user who received the message
    /// </summary>
    public User Receiver { get; set; }

    /// <summary>
    /// Text content of the chat message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Date and time when the message was sent
    /// <remarks>
    /// Defaults to the current UTC time when the message is created.
    /// </remarks>
    /// </summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates whether the message has been read by the receiver
    /// <remarks>
    /// If true, the message has been read; if false, it has not been read
    /// </remarks>
    /// </summary>
    public bool IsRead { get; set; }
}