using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IArtworkService
{
    IEnumerable<ArtworkResponseDTO> GetAll();
    ArtworkResponseDTO? GetById(int id);
    void Add(ArtworkRequestDTO artworkDto);
    void Update(int id, ArtworkRequestDTO artworkDto);
    void Delete(int id);
}
