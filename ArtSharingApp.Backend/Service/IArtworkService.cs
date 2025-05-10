using ArtSharingApp.Backend.Models;
using System.Collections.Generic;

namespace ArtSharingApp.Backend.Service;

public interface IArtworkService
{
    IEnumerable<Artwork> GetAll();
    Artwork? GetById(int id);
    void Add(Artwork artwork);
    void Update(int id, Artwork artwork);
    void Delete(int id);
}
