using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing cities.
/// </summary>
public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CityService"/> class.
    /// </summary>
    /// <param name="cityRepository">Repository for city data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    public CityService(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all cities.
    /// </summary>
    /// <returns>A collection of <see cref="CityResponseDTO"/> representing all cities.</returns>
    public async Task<IEnumerable<CityResponseDTO>> GetAllAsync()
    {
        var cities = await _cityRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CityResponseDTO>>(cities);
    }

    /// <summary>
    /// Retrieves a city by its ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <returns>The <see cref="CityResponseDTO"/> for the specified city.</returns>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    public async Task<CityResponseDTO?> GetByIdAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
            throw new NotFoundException("City with id {id} not found");
        return _mapper.Map<CityResponseDTO>(city);
    }

    /// <summary>
    /// Adds a new city.
    /// </summary>
    /// <param name="cityDto">The city data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    public async Task AddAsync(CityRequestDTO cityDto)
    {
        if (cityDto == null || string.IsNullOrWhiteSpace(cityDto.Name) || string.IsNullOrWhiteSpace(cityDto.Country))
            throw new BadRequestException("City parameters not provided correctly.");
        var city = _mapper.Map<City>(cityDto);
        await _cityRepository.AddAsync(city);
        await _cityRepository.SaveAsync();
    }

    /// <summary>
    /// Updates an existing city.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <param name="cityDto">The updated city data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    public async Task UpdateAsync(int id, CityRequestDTO cityDto)
    {
        if (cityDto == null || string.IsNullOrWhiteSpace(cityDto.Name) || string.IsNullOrWhiteSpace(cityDto.Country))
            throw new BadRequestException("City parameters not provided correctly.");

        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
            throw new NotFoundException($"City with id {id} not found.");

        _mapper.Map(cityDto, city);
        _cityRepository.Update(city);
        await _cityRepository.SaveAsync();
    }

    /// <summary>
    /// Deletes a city by its ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
            throw new NotFoundException($"City with id {id} not found.");
        await _cityRepository.DeleteAsync(id);
        await _cityRepository.SaveAsync();
    }

    /// <summary>
    /// Searches cities by name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>A collection of <see cref="CityResponseDTO"/> matching the name, or null if none found.</returns>
    /// <exception cref="BadRequestException">Thrown if the name is not provided.</exception>
    public async Task<IEnumerable<CityResponseDTO>?> GetCitiesByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Name parameter is required.");
        var cities = await _cityRepository.GetCitiesByName(name);
        if (cities == null)
            return null;
        return _mapper.Map<IEnumerable<CityResponseDTO>>(cities);
    }

    /// <summary>
    /// Retrieves artworks associated with a city by city ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <returns>A collection of <see cref="ArtworkResponseDTO"/> for the city.</returns>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    public async Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByCityId(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id, c => c.Artworks);
        if (city == null)
            throw new NotFoundException($"City with id {id} not found.");
        var artworks = city.Artworks;
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    /// <summary>
    /// Retrieves galleries associated with a city by city ID.
    /// </summary>
    /// <param name="id">The city ID.</param>
    /// <returns>A collection of <see cref="GalleryResponseDTO"/> for the city.</returns>
    /// <exception cref="NotFoundException">Thrown if the city is not found.</exception>
    public async Task<IEnumerable<GalleryResponseDTO>?> GetGalleriesByCityId(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id, c => c.Galleries);
        if (city == null)
            throw new NotFoundException($"City with id {id} not found.");
        var galleries = city.Galleries;
        return _mapper.Map<IEnumerable<GalleryResponseDTO>>(galleries);
    }
}
