using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface ICityService
{
    IEnumerable<City> GetAll();
    City? GetById(int id);
    void Add(City city);
    void Update(int id, City city);
    void Delete(int id);
}