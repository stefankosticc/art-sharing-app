using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

public class CityService : ICityService
{
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IMapper _mapper;

    public CityService(IGenericRepository<City> cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CityResponseDTO>> GetAllAsync()
    {
        var cities = await _cityRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CityResponseDTO>>(cities);
    }

    public async Task<CityResponseDTO?> GetByIdAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
            throw new NotFoundException("City with id {id} not found");
        return _mapper.Map<CityResponseDTO>(city);
    }

    public async Task AddAsync(CityRequestDTO cityDto)
    {
        if (cityDto == null || string.IsNullOrWhiteSpace(cityDto.Name) || string.IsNullOrWhiteSpace(cityDto.Country))
            throw new BadRequestException("City parameters not provided correctly.");
        var city = _mapper.Map<City>(cityDto);
        await _cityRepository.AddAsync(city);
        await _cityRepository.SaveAsync();
    }

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

    public async Task DeleteAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) 
            throw new NotFoundException($"City with id {id} not found.");
        await _cityRepository.DeleteAsync(id);
        await _cityRepository.SaveAsync();
    }
}
