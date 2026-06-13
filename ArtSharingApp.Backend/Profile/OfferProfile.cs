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
    }
}