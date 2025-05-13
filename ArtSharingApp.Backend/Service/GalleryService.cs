using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

public class GalleryService : IGalleryService
{
    private readonly IGalleryRepository _galleryRepository;
    private readonly IMapper _mapper;
    
    public GalleryService(IGalleryRepository galleryRepository, IMapper mapper)
    {
        _galleryRepository = galleryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GalleryResponseDTO>> GetAllAsync()
    {
        var galleries = await _galleryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GalleryResponseDTO>>(galleries);
    }

    public async Task<GalleryResponseDTO?> GetByIdAsync(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id);
        if (gallery == null)
            return null;
        return _mapper.Map<GalleryResponseDTO>(gallery);
    }

    public async Task AddAsync(GalleryRequestDTO galleryDto)
    {
        var gallery = _mapper.Map<Gallery>(galleryDto);
        await _galleryRepository.AddAsync(gallery);
        await _galleryRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, GalleryRequestDTO galleryDto)
    {
        if ( await _galleryRepository.GetByIdAsync(id) == null) return;
        var gallery = _mapper.Map<Gallery>(galleryDto);
        gallery.Id = id;
        _galleryRepository.Update(gallery);
        await _galleryRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id);
        if (gallery == null) return;
        await _galleryRepository.DeleteAsync(id);
        await _galleryRepository.SaveAsync();
    }

    public async Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByGalleryId(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id, g => g.Artworks, g => g.City);
        if (gallery == null) return null;
        var artworks = gallery.Artworks;
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    public async Task<IEnumerable<GalleryResponseDTO>?> GetGalleriesByName(string name)
    {
        var galleries = await _galleryRepository.GetGalleriesByName(name);
        if (galleries == null) return null;
        return _mapper.Map<IEnumerable<GalleryResponseDTO>>(galleries);
    }
}
