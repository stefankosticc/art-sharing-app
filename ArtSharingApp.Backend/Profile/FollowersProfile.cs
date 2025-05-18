using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class FollowersProfile : AutoMapper.Profile
{
    public FollowersProfile()
    {
        CreateMap<Followers, FollowersDTO>()
            .ForMember(dest => dest.FollowerUserName, opt =>
                opt.MapFrom(src => src.User.UserName));
        
        CreateMap<Followers, FollowingDTO>()
            .ForMember(dest => dest.FollowingUserName, opt =>
                opt.MapFrom(src => src.Follower.UserName));
    }
}