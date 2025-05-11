using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IArtworkService
{
    IEnumerable<Artwork> GetAll();
    Artwork? GetById(int id);
    void Add(Artwork artwork);
    void Update(int id, Artwork artwork);
    void Delete(int id);
}
