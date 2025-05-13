using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IGalleryService
{
    Task<IEnumerable<GalleryResponseDTO>> GetAllAsync();
    Task<GalleryResponseDTO?> GetByIdAsync(int id);
    Task AddAsync(GalleryRequestDTO galleryDto);
    Task UpdateAsync(int id, GalleryRequestDTO galleryDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<ArtworkResponseDTO>?> GetArtworksByGalleryId(int id);
    Task<IEnumerable<GalleryResponseDTO>?> GetGalleriesByName(string name);
}
