using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

public class Offer
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public OfferStatus Status { get; set; }
    public int AuctionId { get; set; }
    public Auction Auction { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}