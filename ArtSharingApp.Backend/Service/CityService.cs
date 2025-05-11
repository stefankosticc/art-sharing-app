using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
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

    public IEnumerable<CityResponseDTO> GetAll()
    {
        var cities = _cityRepository.GetAll();
        return _mapper.Map<IEnumerable<CityResponseDTO>>(cities);
    }

    public CityResponseDTO? GetById(int id)
    {
        var city = _cityRepository.GetById(id);
        if (city == null)
            return null;
        return _mapper.Map<CityResponseDTO>(city);
    }

    public void Add(CityRequestDTO cityDto)
    {
        var city = _mapper.Map<City>(cityDto);
        _cityRepository.Add(city);
        _cityRepository.Save();
    }

    public void Update(int id, CityRequestDTO cityDto)
    {
        if (_cityRepository.GetById(id) == null) return;
        var city = _mapper.Map<City>(cityDto);
        city.Id = id;
        _cityRepository.Update(city);
        _cityRepository.Save();
    }

    public void Delete(int id)
    {
        var city = _cityRepository.GetById(id);
        if (city == null) return;
        _cityRepository.Delete(city);
        _cityRepository.Save();
    }
}
