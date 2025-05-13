using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using InvalidOperationException = System.InvalidOperationException;

namespace ArtSharingApp.Backend.Service;

public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly IUserService _userRepository;
    private readonly IArtworkRepository _artworkRepository;
    private readonly IMapper _mapper;

    public FavoritesService(
        IFavoritesRepository favoritesRepository,
        IUserService userRepository,
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
            throw new InvalidOperationException("Artwork already liked by this user.");

        var user = await _userRepository.GetUserByIdAsync(userId);
        var artwork = await _artworkRepository.GetByIdAsync(artworkId);
        if (user == null || artwork == null)
            return false;

        await _favoritesRepository.AddAsync(new Favorites(userId, artworkId));
        await _favoritesRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DislikeArtwork(int userId, int artworkId)
    {
        var liked = (await _favoritesRepository.GetAllAsync())
            .Any(f => f.UserId == userId && f.ArtworkId == artworkId);
        if (!liked)
            throw new InvalidOperationException("Artwork not liked by this user.");

        var user = await _userRepository.GetUserByIdAsync(userId);
        var artwork = await _artworkRepository.GetByIdAsync(artworkId);
        if (user == null || artwork == null)
            return false;
        
        await _favoritesRepository.DeleteAsync(userId, artworkId);
        await _favoritesRepository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<FavoritesDTO>?> GetLikedArtworks(int userId)
    {
        if (await _userRepository.GetUserByIdAsync(userId) == null)
            return null;
        
        var likedArtworks = await _favoritesRepository.GetLikedArtworks(userId);
        if (likedArtworks == null || !likedArtworks.Any())
            return null;
        var likedArtworksDto = _mapper.Map<List<FavoritesDTO>>(likedArtworks);
        return likedArtworksDto;
    }
}

