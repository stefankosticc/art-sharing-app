using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Utils;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing artworks.
/// </summary>
public class ArtworkService : IArtworkService
{
    private readonly IArtworkRepository _artworkRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IFavoritesRepository _favoritesRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtworkService"/> class.
    /// </summary>
    /// <param name="artworkRepository">Repository for artwork data access.</param>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    /// <param name="favoritesRepository">Repository for favorites data access.</param>
    public ArtworkService(IArtworkRepository artworkRepository, IUserRepository userRepository, IMapper mapper,
        IFavoritesRepository favoritesRepository)
    {
        _artworkRepository = artworkRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _favoritesRepository = favoritesRepository;
    }

    /// <summary>
    /// Retrieves all artworks.
    /// </summary>
    /// <returns>A collection of <see cref="ArtworkResponseDTO"/> representing all artworks.</returns>
    public async Task<IEnumerable<ArtworkResponseDTO>> GetAllAsync()
    {
        var artworks = await _artworkRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    /// <summary>
    /// Retrieves an artwork by its ID.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <returns>The <see cref="ArtworkResponseDTO"/> for the specified artwork.</returns>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
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

    /// <summary>
    /// Adds a new artwork.
    /// </summary>
    /// <param name="artworkDto">The artwork data.</param>
    /// <param name="artworkImage">The image file for the artwork.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
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

    /// <summary>
    /// Updates an existing artwork.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="artworkDto">The updated artwork data.</param>
    /// <param name="artworkImage">The new image file for the artwork (optional).</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
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

    /// <summary>
    /// Deletes an artwork by its ID.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        await _artworkRepository.DeleteAsync(id);
        await _artworkRepository.SaveAsync();
    }

    /// <summary>
    /// Searches artworks by title.
    /// </summary>
    /// <param name="title">The title to search for.</param>
    /// <returns>A collection of <see cref="ArtworkSearchResponseDTO"/> matching the title.</returns>
    /// <exception cref="BadRequestException">Thrown if the title is invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if no artworks are found.</exception>
    public async Task<IEnumerable<ArtworkSearchResponseDTO>?> SearchByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new BadRequestException("Title parameter is required.");
        var artworks = await _artworkRepository.SearchByTitle(title);
        if (artworks == null || !artworks.Any())
            throw new NotFoundException($"No artworks found with this title.");
        return _mapper.Map<IEnumerable<ArtworkSearchResponseDTO>>(artworks);
    }

    /// <summary>
    /// Changes the visibility of an artwork.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="isPrivate">True to set the artwork as private, false for public.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    public async Task ChangeVisibilityAsync(int id, bool isPrivate)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        artwork.ChangeVisibility(isPrivate);
        _artworkRepository.UpdateIsPrivate(artwork);
        await _artworkRepository.SaveAsync();
    }

    /// <summary>
    /// Puts an artwork on sale with fixed price.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="request">The sale details.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to put the artwork on sale.</exception>
    /// <exception cref="BadRequestException">Thrown if sale operation fails.</exception>
    public async Task PutOnSaleAsync(int id, int loggedInUserId, PutArtworkOnSaleDTO request)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");

        if (artwork.PostedByUserId != loggedInUserId)
            throw new UnauthorizedAccessException("You are not authorized to put this artwork on sale.");

        try
        {
            artwork.PutOnSale(request.Price, request.Currency);
        }
        catch (Exception e)
        {
            throw new BadRequestException($"Failed to put artwork on sale: {e.Message}");
        }

        _artworkRepository.UpdateSaleProperties(artwork);
        await _artworkRepository.SaveAsync();
    }

    /// <summary>
    /// Removes an artwork from sale.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to remove the artwork from sale.</exception>
    public async Task RemoveFromSaleAsync(int id, int loggedInUserId)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");

        if (artwork.PostedByUserId != loggedInUserId)
            throw new UnauthorizedAccessException("You are not authorized to remove this artwork from sale.");

        artwork.RemoveFromSale();
        _artworkRepository.UpdateSaleProperties(artwork);
        await _artworkRepository.SaveAsync();
    }

    /// <summary>
    /// Transfers ownership of an artwork to another user.
    /// </summary>
    /// <param name="artworkId">The artwork ID.</param>
    /// <param name="fromUserId">The current owner's user ID.</param>
    /// <param name="toUserId">The new owner's user ID.</param>
    /// <exception cref="NotFoundException">Thrown if artwork or user is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to transfer the artwork.</exception>
    /// <exception cref="BadRequestException">Thrown if transfer is not needed.</exception>
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

        artwork.TransferOwnership(toUserId);
        _artworkRepository.UpdateOwner(artwork);
        await _artworkRepository.SaveAsync();
    }

    /// <summary>
    /// Gets all artworks for a user, including private artworks if the user is the logged-in user.
    /// </summary>
    /// <param name="userId">The user ID whose artworks to retrieve.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <returns>A <see cref="UserArtworksDTO"/> containing public and private artworks.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
    public async Task<UserArtworksDTO> GetUserArtworksAsync(int userId, int loggedInUserId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException($"User with id {userId} not found.");

        var response = new UserArtworksDTO();

        if (userId == loggedInUserId)
        {
            var privateArtworks = await _artworkRepository.GetPrivateArtworksByUserAsync(userId);
            response.PrivateArtworks = _mapper.Map<IEnumerable<ArtworkPreviewDTO>>(privateArtworks);
        }

        var publicArtworks = await _artworkRepository.GetPublicArtworksByUserAsync(userId);
        response.PublicArtworks = _mapper.Map<IEnumerable<ArtworkPreviewDTO>>(publicArtworks);

        return response;
    }

    /// <summary>
    /// Retrieves the image and content type for an artwork.
    /// </summary>
    /// <param name="id">The artwork ID.</param>
    /// <returns>A tuple containing the image bytes and content type.</returns>
    /// <exception cref="NotFoundException">Thrown if the image is not found.</exception>
    public async Task<(byte[] Image, string ContentType)> GetArtworkImageAsync(int id)
    {
        var result = await _artworkRepository.GetArtworkImageAsync(id);
        if (result.Image == null || result.Image.Length == 0)
            throw new NotFoundException("Image not found.");

        return (result.Image, string.IsNullOrWhiteSpace(result.ContentType) ? "image/jpeg" : result.ContentType);
    }

    /// <summary>
    /// Extracts the dominant color from an image file.
    /// </summary>
    /// <param name="image">The image file.</param>
    /// <returns>The extracted color as a hex string, or null if extraction fails.</returns>
    /// <exception cref="BadRequestException">Thrown if the image is invalid.</exception>
    public async Task<string?> ExtractColorAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new BadRequestException("Image is required.");

        byte[] imageBytes;
        using (var ms = new MemoryStream())
        {
            await image.CopyToAsync(ms);
            imageBytes = ms.ToArray();
        }

        try
        {
            var color = ImageColorHelper.ExtractSaturationWeightedAverageColor(imageBytes);
            return color;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Retrieves artworks for discovery, excluding those owned or liked by the logged-in user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="skip">The number of artworks to skip.</param>
    /// <param name="take">The number of artworks to take.</param>
    /// <returns>A collection of <see cref="DiscoverArtworkDTO"/> for discovery.</returns>
    public async Task<IEnumerable<DiscoverArtworkDTO>?> GetDiscoverArtworksAsync(int loggedInUserId, int skip, int take)
    {
        var artworks = await _artworkRepository.GetDiscoverArtworksAsync(loggedInUserId, skip, take);
        return _mapper.Map<IEnumerable<DiscoverArtworkDTO>>(artworks);
    }
}