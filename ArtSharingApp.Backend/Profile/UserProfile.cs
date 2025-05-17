using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class UserProfile : AutoMapper.Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.RoleName, opt => 
                opt.MapFrom(src => src.Role != null ? src.Role.Name : null));

        CreateMap<UserRequestDTO, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        CreateMap<UserRegisterDTO, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());
    }
}
