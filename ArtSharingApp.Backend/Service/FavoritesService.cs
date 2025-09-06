using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using InvalidOperationException = System.InvalidOperationException;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing artwork favorites (likes).
/// </summary>
public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IArtworkRepository _artworkRepository;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoritesService"/> class.
    /// </summary>
    /// <param name="favoritesRepository">Repository for favorites data access.</param>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="artworkRepository">Repository for artwork data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    /// <param name="notificationService">Service for sending notifications.</param>
    public FavoritesService(
        IFavoritesRepository favoritesRepository,
        IUserRepository userRepository,
        IArtworkRepository artworkRepository,
        IMapper mapper,
        INotificationService notificationService)
    {
        _favoritesRepository = favoritesRepository;
        _userRepository = userRepository;
        _artworkRepository = artworkRepository;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Likes an artwork for a user.
    /// </summary>
    /// <param name="userId">The ID of the user liking the artwork.</param>
    /// <param name="artworkId">The ID of the artwork to like.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if the artwork is already liked by the user.</exception>
    /// <exception cref="NotFoundException">Thrown if the user or artwork is not found.</exception>
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

        // Create notification
        if (artwork.PostedByUserId != userId)
        {
            var notification = new NotificationRequestDTO
            {
                RecipientId = artwork.PostedByUserId,
                Text = $"@{user.UserName} liked '{artwork.Title}'."
            };
            await _notificationService.CreateNotificationAsync(notification);
        }

        return true;
    }

    /// <summary>
    /// Removes a like from an artwork for a user.
    /// </summary>
    /// <param name="userId">The ID of the user disliking the artwork.</param>
    /// <param name="artworkId">The ID of the artwork to dislike.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if the artwork is not liked by the user.</exception>
    /// <exception cref="NotFoundException">Thrown if the user or artwork is not found.</exception>
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

    /// <summary>
    /// Retrieves all artworks liked by a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of <see cref="FavoritesDTO"/> representing liked artworks.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found or has no liked artworks.</exception>
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

    /// <summary>
    /// Retrieves the top 10 artists by number of likes.
    /// </summary>
    /// <remarks>
    /// Top artists are determined based on the total number of likes their artworks have received.
    /// </remarks>
    /// <returns>A collection of <see cref="TopArtistResponseDTO"/> representing the top artists.</returns>
    public async Task<IEnumerable<TopArtistResponseDTO>?> GetTop10ArtistsByLikesAsync()
    {
        var artists = await _favoritesRepository.GetTopArtistsByLikesAsync(10);
        return _mapper.Map<IEnumerable<TopArtistResponseDTO>>(artists);
    }

    /// <summary>
    /// Retrieves trending artworks based on likes in the last 30 days.
    /// </summary>
    /// <remarks>
    /// Trending artworks are determined based on the number of likes received in the last 30 days.
    /// </remarks>
    /// <param name="count">The number of trending artworks to retrieve.</param>
    /// <returns>A collection of <see cref="DiscoverArtworkDTO"/> representing trending artworks.</returns>
    public async Task<IEnumerable<DiscoverArtworkDTO>?> GetTrendingArtworksAsync(int count)
    {
        var fromDate = DateTime.UtcNow.AddDays(-30);
        var artworks = await _favoritesRepository.GetTrendingArtworksAsync(fromDate, count);
        return _mapper.Map<IEnumerable<DiscoverArtworkDTO>>(artworks);
    }
}
