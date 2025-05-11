using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;

namespace ArtSharingApp.Backend.Service;

public class CityService : ICityService
{
    private readonly IGenericRepository<City> _cityRepository;

    public CityService(IGenericRepository<City> cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public IEnumerable<City> GetAll()
    {
        return _cityRepository.GetAll();
    }

    public City GetById(int id)
    {
        return _cityRepository.GetById(id);
    }

    public void Add(City city)
    {
        _cityRepository.Add(city);
        _cityRepository.Save();
    }

    public void Update(int id, City city)
    {
        if (_cityRepository.GetById(id) == null) return;
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