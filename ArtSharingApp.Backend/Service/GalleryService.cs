using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing galleries.
/// </summary>
public class GalleryService : IGalleryService
{
    private readonly IGalleryRepository _galleryRepository;
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GalleryService"/> class.
    /// </summary>
    /// <param name="galleryRepository">Repository for gallery data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    /// <param name="cityRepository">Repository for city data access.</param>
    public GalleryService(IGalleryRepository galleryRepository, IMapper mapper, IGenericRepository<City> cityRepository)
    {
        _galleryRepository = galleryRepository;
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all galleries.
    /// </summary>
    /// <returns>A collection of <see cref="GalleryResponseDTO"/> representing all galleries.</returns>
    public async Task<IEnumerable<GalleryResponseDTO>> GetAllAsync()
    {
        var galleries = await _galleryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GalleryResponseDTO>>(galleries);
    }

    /// <summary>
    /// Retrieves a gallery by its ID.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <returns>The <see cref="GalleryResponseDTO"/> for the specified gallery.</returns>
    /// <exception cref="NotFoundException">Thrown if the gallery is not found.</exception>
    public async Task<GalleryResponseDTO?> GetByIdAsync(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id, g => g.City);
        if (gallery == null)
            throw new NotFoundException($"Gallery with id {id} not found.");
        return _mapper.Map<GalleryResponseDTO>(gallery);
    }

    /// <summary>
    /// Adds a new gallery.
    /// </summary>
    /// <param name="galleryDto">The gallery data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
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

    /// <summary>
    /// Updates an existing gallery.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <param name="galleryDto">The updated gallery data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the gallery or city is not found.</exception>
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

    /// <summary>
    /// Deletes a gallery by its ID.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <exception cref="NotFoundException">Thrown if the gallery is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id);
        if (gallery == null)
            throw new NotFoundException($"Gallery with id {id} not found.");
        await _galleryRepository.DeleteAsync(id);
        await _galleryRepository.SaveAsync();
    }

    /// <summary>
    /// Retrieves artworks associated with a gallery by gallery ID.
    /// </summary>
    /// <param name="id">The gallery ID.</param>
    /// <returns>A collection of <see cref="ArtworkResponseDTO"/> representing artworks in the gallery.</returns>
    /// <exception cref="NotFoundException">Thrown if the gallery is not found.</exception>
    public async Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByGalleryId(int id)
    {
        var gallery = await _galleryRepository.GetByIdAsync(id, g => g.Artworks, g => g.City);
        if (gallery == null)
            throw new NotFoundException($"Gallery with id {id} not found.");
        var artworks = gallery.Artworks;
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    /// <summary>
    /// Searches galleries by name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>A collection of <see cref="GalleryResponseDTO"/> matching the name, or null if none found.</returns>
    /// <exception cref="BadRequestException">Thrown if the name is not provided.</exception>
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
