using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.DTO;

public class OfferRequestDTO
{
    public decimal Amount { get; set; }
}

public class OfferResponseDTO
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public decimal Amount { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string? UserProfilePhoto { get; set; }
    public OfferStatus Status { get; set; }
}

public class MyOfferDTO
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int ArtworkId { get; set; }
    public string ArtworkTitle { get; set; }
    public string ArtworkImage { get; set; }
    public string PostedByUserName { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public OfferStatus Status { get; set; }
    public DateTime AuctionEndTime { get; set; }
}
