using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Utils;

namespace ArtSharingApp.Backend.Profile;

public class AuctionProfile : AutoMapper.Profile
{
    public AuctionProfile()
    {
        CreateMap<AuctionStartDTO, Auction>()
            .ForMember(dest => dest.Artwork, opt => opt.Ignore());

        CreateMap<Auction, ActiveAuctionDTO>()
            .ForMember(dest => dest.AuctionId, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ArtworkTitle, opt =>
                opt.MapFrom(src => src.Artwork.Title))
            .ForMember(dest => dest.ArtworkImage, opt =>
                opt.MapFrom(src => src.Artwork.ImageId != null ? ImagePaths.Artwork(src.Artwork.ImageId) : null))
            .ForMember(dest => dest.PostedByUserName, opt =>
                opt.MapFrom(src => src.Artwork.PostedByUser.UserName))
            .ForMember(dest => dest.CurrentPrice, opt =>
                opt.MapFrom(src => src.Offers.Any() ? src.Offers.Max(o => o.Amount) : src.StartingPrice))
            .ForMember(dest => dest.OfferCount, opt =>
                opt.MapFrom(src => src.Offers.Count));
    }
}