using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents an auction for an artwork
/// </summary>
public class Auction
{
    /// <summary>
    /// Unique identifier for the auction
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Start time of the auction
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End time of the auction
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Starting price of the auction
    /// <remarks>
    /// This is the minimum price at which the auction starts.
    /// </remarks>
    /// </summary>
    public decimal StartingPrice { get; set; }

    /// <summary>
    /// Currency used for the auction. See <see cref="Currency"/>.
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// Unique identifier for the artwork on which the auction is held
    /// </summary>
    public int ArtworkId { get; set; }

    /// <summary>
    /// Navigation property to the artwork associated with the auction
    /// </summary>
    public Artwork Artwork { get; set; }

    /// <summary>
    /// Collection of offers made on the auction
    /// </summary>
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}