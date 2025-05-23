using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models.Enums;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

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
            throw new NotFoundException($"Artwork with id {id} not found.");
        return _mapper.Map<ArtworkResponseDTO>(artwork);
    }

    public async Task AddAsync(ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            throw new BadRequestException("Artwork parameters not provided correctly.");
        var artwork = _mapper.Map<Artwork>(artworkDto);
        await _artworkRepository.AddAsync(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            throw new BadRequestException("Artwork parameters not provided correctly.");
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        _mapper.Map(artworkDto, artwork);
        _artworkRepository.Update(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        await _artworkRepository.DeleteAsync(id);
        await _artworkRepository.SaveAsync();
    }

    public async Task<IEnumerable<ArtworkResponseDTO>?> SearchByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new BadRequestException("Title parameter is required.");
        var artworks = await _artworkRepository.SearchByTitle(title);
        if (artworks == null || !artworks.Any())
            throw new NotFoundException($"No artworks found with this title.");
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    public async Task ChangeVisibilityAsync(int id, bool isPrivate)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        artwork.IsPrivate = isPrivate;
        _artworkRepository.UpdateIsPrivate(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task PutOnSaleAsync(int id, int loggedInUserId, PutArtworkOnSaleDTO request)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        
        if (artwork.PostedByUserId != loggedInUserId)
            throw new UnauthorizedAccessException("You are not authorized to put this artwork on sale.");
        
        artwork.IsOnSale = request.IsOnSale;
        artwork.Price = request.Price;
        artwork.Currency = request.Currency;
        _artworkRepository.UpdateSaleProperties(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task RemoveFromSaleAsync(int id, int loggedInUserId)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        
        if (artwork.PostedByUserId != loggedInUserId)
            throw new UnauthorizedAccessException("You are not authorized to remove this artwork from sale.");
        
        artwork.IsOnSale = false;
        _artworkRepository.UpdateSaleProperties(artwork);
        await _artworkRepository.SaveAsync();
    }
}
