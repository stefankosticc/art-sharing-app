using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Utils;

namespace ArtSharingApp.Backend.Profile;

public class UserProfile : AutoMapper.Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.RoleName, opt =>
                opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => ImagePaths.UserProfilePhoto(src.ProfilePhotoId)));

        CreateMap<UserRequestDTO, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        CreateMap<UserRegisterDTO, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        CreateMap<User, UserSearchResponseDTO>()
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => ImagePaths.UserProfilePhoto(src.ProfilePhotoId)));

        CreateMap<User, TopArtistResponseDTO>()
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => ImagePaths.UserProfilePhoto(src.ProfilePhotoId)));

        CreateMap<User, UserByUserNameResponseDTO>()
            .ForMember(dest => dest.ProfilePhoto, opt =>
                opt.MapFrom(src => ImagePaths.UserProfilePhoto(src.ProfilePhotoId)));

        CreateMap<UpdateUserProfileRequestDTO, User>();
    }
}
