using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using InvalidOperationException = System.InvalidOperationException;

namespace ArtSharingApp.Backend.Service;

public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArtworkRepository _artworkRepository;
    private readonly IMapper _mapper;

    public FavoritesService(
        IFavoritesRepository favoritesRepository,
        IUserRepository userRepository,
        IArtworkRepository artworkRepository,
        IMapper mapper)
    {
        _favoritesRepository = favoritesRepository;
        _userRepository = userRepository;
        _artworkRepository = artworkRepository;
        _mapper = mapper;
    }

    public async Task<bool> LikeArtwork(int userId, int artworkId)
    {
        var alreadyLiked = (await _favoritesRepository.GetAllAsync())
            .Any(f => f.UserId == userId && f.ArtworkId == artworkId);
        if (alreadyLiked)
            throw new BadRequestException("Artwork already liked by this user.");

        var user = await _userRepository.GetByIdAsync(userId);
        var artwork = await _artworkRepository.GetByIdAsync(artworkId);
        if (user == null || artwork == null)
            throw new NotFoundException("User or artwork not found.");

        await _favoritesRepository.AddAsync(new Favorites(userId, artworkId));
        await _favoritesRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DislikeArtwork(int userId, int artworkId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var artwork = await _artworkRepository.GetByIdAsync(artworkId);
        if (user == null || artwork == null)
            throw new NotFoundException("User or artwork not found.");
        
        var liked = (await _favoritesRepository.GetAllAsync())
            .Any(f => f.UserId == userId && f.ArtworkId == artworkId);
        if (!liked)
            throw new BadRequestException("Artwork not liked by this user.");
        
        await _favoritesRepository.DeleteAsync(userId, artworkId);
        await _favoritesRepository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<FavoritesDTO>?> GetLikedArtworks(int userId)
    {
        if (await _userRepository.GetByIdAsync(userId) == null)
            throw new NotFoundException($"User with id {userId} not found.");
        
        var likedArtworks = await _favoritesRepository.GetLikedArtworks(userId);
        if (likedArtworks == null || !likedArtworks.Any())
            throw new NotFoundException($"No liked artworks found for user with id {userId}.");
        var likedArtworksDto = _mapper.Map<List<FavoritesDTO>>(likedArtworks);
        return likedArtworksDto;
    }
}

