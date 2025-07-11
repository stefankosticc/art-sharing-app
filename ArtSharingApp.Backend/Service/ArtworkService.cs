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
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IFavoritesRepository _favoritesRepository;

    public ArtworkService(IArtworkRepository artworkRepository, IUserRepository userRepository, IMapper mapper, IFavoritesRepository favoritesRepository)
    {
        _artworkRepository = artworkRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _favoritesRepository = favoritesRepository;
    }

    public async Task<IEnumerable<ArtworkResponseDTO>> GetAllAsync()
    {
        var artworks = await _artworkRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    public async Task<ArtworkResponseDTO?> GetByIdAsync(int id, int loggedInUserId)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        var response = _mapper.Map<ArtworkResponseDTO>(artwork);
        response.IsLikedByLoggedInUser = (await _favoritesRepository.GetAllAsync())
            .Any(f => f.UserId == loggedInUserId && f.ArtworkId == id);
        return response;
    }

    public async Task AddAsync(ArtworkRequestDTO artworkDto, IFormFile artworkImage)
    {
        if (artworkDto == null)
            throw new BadRequestException("Artwork parameters not provided correctly.");
        if (artworkImage == null || artworkImage.Length == 0)
            throw new BadRequestException("Image not provided correctly.");
        
        var artwork = _mapper.Map<Artwork>(artworkDto);

        using (var ms = new MemoryStream())
        {
            await artworkImage.CopyToAsync(ms);
            artwork.Image = ms.ToArray();
        }
        artwork.ContentType = artworkImage.ContentType;
        
        await _artworkRepository.AddAsync(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task UpdateAsync(int id, ArtworkRequestDTO artworkDto, IFormFile? artworkImage)
    {
        if (artworkDto == null)
            throw new BadRequestException("Artwork parameters not provided correctly.");
        
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        
        _mapper.Map(artworkDto, artwork);

        if (artworkImage != null && artworkImage.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                await artworkImage.CopyToAsync(ms);
                artwork.Image = ms.ToArray();
            }

            artwork.ContentType = artworkImage.ContentType;
        }

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

    public async Task<IEnumerable<ArtworkSearchResponseDTO>?> SearchByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new BadRequestException("Title parameter is required.");
        var artworks = await _artworkRepository.SearchByTitle(title);
        if (artworks == null || !artworks.Any())
            throw new NotFoundException($"No artworks found with this title.");
        return _mapper.Map<IEnumerable<ArtworkSearchResponseDTO>>(artworks);
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

    public async Task TransferToUserAsync(int artworkId, int fromUserId, int toUserId)
    {
        var artwork = await _artworkRepository.GetByIdAsync(artworkId);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {artworkId} not found.");
        
        if (artwork.PostedByUserId != fromUserId)
            throw new UnauthorizedAccessException("You are not authorized to transfer this artwork.");
        
        var toUser = await _userRepository.GetByIdAsync(toUserId);
        if (toUser == null)
            throw new NotFoundException($"User with id {toUserId} not found.");
        
        if (fromUserId == toUserId)
            throw new BadRequestException("You already own this artwork, no transfer needed.");
        
        artwork.PostedByUserId = toUserId;
        _artworkRepository.UpdateOwner(artwork);
        await _artworkRepository.SaveAsync();
    }

    public async Task<IEnumerable<ArtworkPreviewDTO>?> GetMyArtworksAsync(int loggedInUserId)
    {
        var artworks = await _artworkRepository.GetMyArtworksAsync(loggedInUserId);
        return _mapper.Map<IEnumerable<ArtworkPreviewDTO>>(artworks);
    }

    public async Task<(byte[] Image, string ContentType)> GetArtworkImageAsync(int id)
    {
        var result = await _artworkRepository.GetArtworkImageAsync(id);
        if (result.Image == null || result.Image.Length == 0)
            throw new NotFoundException("Image not found.");

        return (result.Image, string.IsNullOrWhiteSpace(result.ContentType) ? "image/jpeg" : result.ContentType);
    }
}
