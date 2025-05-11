using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface ICityService
{
    IEnumerable<CityResponseDTO> GetAll();
    CityResponseDTO? GetById(int id);
    void Add(CityRequestDTO cityDto);
    void Update(int id, CityRequestDTO cityDto);
    void Delete(int id);
}
