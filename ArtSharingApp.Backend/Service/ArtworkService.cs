using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.DataAccess.Repository;
using System.Collections.Generic;

namespace ArtSharingApp.Backend.Service;

public class ArtworkService : IArtworkService
{
    private readonly IArtworkRepository _artworkRepository;

    public ArtworkService(IArtworkRepository artworkRepository)
    {
        _artworkRepository = artworkRepository;
    }

    public IEnumerable<Artwork> GetAll()
    {
        return _artworkRepository.GetAll();
    }

    public Artwork? GetById(int id)
    {
        return _artworkRepository.GetById(id);
    }

    public void Add(Artwork artwork)
    {
        _artworkRepository.Add(artwork);
        _artworkRepository.Save();
    }

    public void Update(int id, Artwork artwork)
    {
        if (_artworkRepository.GetById(id) == null) return;
        _artworkRepository.Update(artwork);
        _artworkRepository.Save();
    }

    public void Delete(int id)
    {
        var artwork = _artworkRepository.GetById(id);
        if (artwork == null) return;
        _artworkRepository.Delete(artwork);
        _artworkRepository.Save();
    }
}
