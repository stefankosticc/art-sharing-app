using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.DTO;

public class AuctionStartDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal StartingPrice { get; set; }
    public Currency Currency { get; set; }
}