using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IGalleryService
{
    IEnumerable<GalleryResponseDTO> GetAll();
    GalleryResponseDTO? GetById(int id);
    void Add(GalleryRequestDTO galleryDto);
    void Update(int id, GalleryRequestDTO galleryDto);
    void Delete(int id);
}
