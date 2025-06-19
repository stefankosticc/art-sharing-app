using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class ArtworkProfile : AutoMapper.Profile
{
    public ArtworkProfile()
    {
        CreateMap<Artwork, ArtworkResponseDTO>()
            .ForMember(dest => dest.CreatedByArtistName, opt => 
                opt.MapFrom(src => src.CreatedByArtist != null ? src.CreatedByArtist.Name : null))
            .ForMember(dest => dest.PostedByUserName, opt =>
                opt.MapFrom(src => src.PostedByUser != null ? src.PostedByUser.UserName : null))
            .ForMember(dest => dest.CityName, opt => 
                opt.MapFrom(src => src.City != null ? src.City.Name : null))
            .ForMember(dest => dest.GalleryName, opt => 
                opt.MapFrom(src => src.Gallery != null ? src.Gallery.Name : null));

        CreateMap<ArtworkRequestDTO, Artwork>()
            .ForMember(dest => dest.CreatedByArtist, opt => opt.Ignore())
            .ForMember(dest => dest.PostedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.Gallery, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore());

        CreateMap<Artwork, ArtworkPreviewDTO>()            
            .ForMember(dest => dest.PostedByUserName, opt =>
            opt.MapFrom(src => src.PostedByUser != null ? src.PostedByUser.UserName : null));
    }
}
