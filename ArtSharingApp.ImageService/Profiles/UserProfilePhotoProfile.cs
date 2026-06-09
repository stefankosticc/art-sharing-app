using ArtSharingApp.ImageService.DTO;
using ArtSharingApp.ImageService.Models;

namespace ArtSharingApp.ImageService.Profiles;

public class UserProfilePhotoProfile : AutoMapper.Profile
{
    public UserProfilePhotoProfile()
    {
        CreateMap<UserProfilePhoto, UserProfilePhotoDTO>()
            .ForMember(dest => dest.ImageId, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Url, opt =>
                opt.MapFrom(src => $"/images/users/profile-photo/{src.Id}"));
    }
}