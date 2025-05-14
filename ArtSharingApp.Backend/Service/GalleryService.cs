using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

public class GalleryService : IGalleryService
{
    private readonly IGalleryRepository _galleryRepository;
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IMapper _mapper;
    
    public GalleryService(IGalleryRepository galleryRepository, IMapper mapper, IGenericRepository<City> cityRepository)
    {
        _galleryRepository = galleryRepository;
        _cityRepository = cityRepository;
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
            throw new NotFoundException($"Gallery with id {id} not found.");
        return _mapper.Map<GalleryResponseDTO>(gallery);
    }

    public async Task AddAsync(GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null || string.IsNullOrWhiteSpace(galleryDto.Name))
            throw new BadRequestException("Gallery parameters not provided correctly.");
        
        if (await _cityRepository.GetByIdAsync(galleryDto.CityId) == null)
            throw new NotFoundException($"City with id {galleryDto.CityId} not found.");
        
        var gallery = _mapper.Map<Gallery>(galleryDto);
        await _galleryRepository.AddAsync(gallery);
        await _galleryRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null || string.IsNullOrWhiteSpace(galleryDto.Name))
            throw new BadRequestException("Gallery parameters not provided correctly.");
        
        var gallery = await _galleryRepository.GetByIdAsync(id);
        if (gallery == null)
            throw new NotFoundException($"Gallery with id {id} not found.");
        
        if (await _cityRepository.GetByIdAsync(galleryDto.CityId) == null)
            throw new NotFoundException($"City with id {galleryDto.CityId} not found.");

        _mapper.Map(galleryDto, gallery);

        _galleryRepository.Update(gallery);
        await _galleryRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id);
        if (gallery == null)
            throw new NotFoundException($"Gallery with id {id} not found.");
        await _galleryRepository.DeleteAsync(id);
        await _galleryRepository.SaveAsync();
    }

    public async Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByGalleryId(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id, g => g.Artworks, g => g.City);
        if (gallery == null)
            throw new NotFoundException($"Gallery with id {id} not found.");
        var artworks = gallery.Artworks;
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    public async Task<IEnumerable<GalleryResponseDTO>?> GetGalleriesByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Name parameter is required.");
        var galleries = await _galleryRepository.GetGalleriesByName(name);
        if (galleries == null)
            return null;
        return _mapper.Map<IEnumerable<GalleryResponseDTO>>(galleries);
    }
}
