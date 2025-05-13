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

    public async Task<IEnumerable<ArtworkResponseDTO>> GetAllAsync()
    {
        var artworks = await _artworkRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    public async Task<ArtworkResponseDTO?> GetByIdAsync(int id)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            return null;
        return _mapper.Map<ArtworkResponseDTO>(artwork);
    }

    public async Task AddAsync(ArtworkRequestDTO artworkDto)
    {
        var artwork = _mapper.Map<Artwork>(artworkDto);
        await _artworkRepository.AddAsync(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, ArtworkRequestDTO artworkDto)
    {
        if (await _artworkRepository.GetByIdAsync(id) == null) return;
        var artwork = _mapper.Map<Artwork>(artworkDto);
        artwork.Id = id;
        _artworkRepository.Update(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null) return;
        await _artworkRepository.DeleteAsync(id);
        await _artworkRepository.SaveAsync();
    }

    public async Task<IEnumerable<ArtworkResponseDTO>?> SearchByTitle(string title)
    {
        var artworks = await _artworkRepository.SearchByTitle(title);
        if (artworks == null)
            return null;
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }
}
