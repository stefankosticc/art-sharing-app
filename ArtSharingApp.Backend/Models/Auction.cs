using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

public class Auction
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal StartingPrice { get; set; }
    public Currency Currency { get; set; }
    public int ArtworkId { get; set; }
    public Artwork Artwork { get; set; }
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    
}