using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Utils;

namespace ArtSharingApp.Backend.Profile;

public class OfferProfile : AutoMapper.Profile
{
    public OfferProfile()
    {
        CreateMap<OfferRequestDTO, Offer>()
            .ForMember(dest => dest.Auction, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Offer, OfferResponseDTO>()
            .ForMember(dest => dest.UserName, opt
                => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.UserProfilePhoto, opt
                => opt.MapFrom(src => src.User.ProfilePhotoId != null
                    ? ImagePaths.UserProfilePhoto(src.User.ProfilePhotoId)
                    : null));

        CreateMap<Offer, MyOfferDTO>()
            .ForMember(dest => dest.AuctionId, opt =>
                opt.MapFrom(src => src.Auction.Id))
            .ForMember(dest => dest.ArtworkId, opt =>
                opt.MapFrom(src => src.Auction.Artwork.Id))
            .ForMember(dest => dest.ArtworkTitle, opt =>
                opt.MapFrom(src => src.Auction.Artwork.Title))
            .ForMember(dest => dest.ArtworkImage, opt =>
                opt.MapFrom(src => src.Auction.Artwork.ImageId != null
                    ? ImagePaths.Artwork(src.Auction.Artwork.ImageId)
                    : null))
            .ForMember(dest => dest.PostedByUserName, opt =>
                opt.MapFrom(src => src.Auction.Artwork.PostedByUser.UserName))
            .ForMember(dest => dest.Currency, opt =>
                opt.MapFrom(src => src.Auction.Currency))
            .ForMember(dest => dest.AuctionEndTime, opt =>
                opt.MapFrom(src => src.Auction.EndTime));
    }
}