using ArtSharingApp.ImageService.DTO;
using ArtSharingApp.ImageService.Models;

namespace ArtSharingApp.ImageService.Profiles;

public class ArtworkImageProfile : AutoMapper.Profile
{
    public ArtworkImageProfile()
    {
        CreateMap<ArtworkImage, ArtworkImageDTO>()
            .ForMember(dest => dest.ImageId, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Url, opt =>
                opt.MapFrom(src => $"/images/artworks/{src.Id}"))
            .ForMember(dest => dest.DominantColor, opt =>
                opt.Ignore());
    }
}