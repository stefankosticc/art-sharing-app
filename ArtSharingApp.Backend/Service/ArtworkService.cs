using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.DataAccess.Repository;
using System.Collections.Generic;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

public class ArtworkService : IArtworkService
{
    private readonly IArtworkRepository _artworkRepository;
    private readonly IMapper _mapper;

    public ArtworkService(IArtworkRepository artworkRepository, IMapper mapper)
    {
        _artworkRepository = artworkRepository;
        _mapper = mapper;
    }

    public IEnumerable<ArtworkResponseDTO> GetAll()
    {
        var artworks = _artworkRepository.GetAll();
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    public ArtworkResponseDTO? GetById(int id)
    {
        var artwork = _artworkRepository.GetById(id);
        if (artwork == null)
            return null;
        return _mapper.Map<ArtworkResponseDTO>(artwork);
    }

    public void Add(ArtworkRequestDTO artworkDto)
    {
        var artwork = _mapper.Map<Artwork>(artworkDto);
        _artworkRepository.Add(artwork);
        _artworkRepository.Save();
    }

    public void Update(int id, ArtworkRequestDTO artworkDto)
    {
        if (_artworkRepository.GetById(id) == null) return;
        var artwork = _mapper.Map<Artwork>(artworkDto);
        artwork.Id = id;
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
