using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface ICityService
{
    Task<IEnumerable<CityResponseDTO>> GetAllAsync();
    Task<CityResponseDTO?> GetByIdAsync(int id);
    Task AddAsync(CityRequestDTO cityDto);
    Task UpdateAsync(int id, CityRequestDTO cityDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<CityResponseDTO>?> GetCitiesByName(string name);
    Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByCityId(int id);
    Task<IEnumerable<GalleryResponseDTO>?> GetGalleriesByCityId(int id);
}
