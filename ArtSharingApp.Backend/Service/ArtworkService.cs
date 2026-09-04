using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Infrastructure;
using ArtSharingApp.Backend.Utils;
using Refit;
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
    private readonly IImageServiceClient _imageServiceClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtworkService"/> class.
    /// </summary>
    /// <param name="artworkRepository">Repository for artwork data access.</param>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    /// <param name="favoritesRepository">Repository for favorites data access.</param>
    /// <param name="imageServiceClient">Client for the image microservice.</param>
    public ArtworkService(IArtworkRepository artworkRepository, IUserRepository userRepository, IMapper mapper,
        IFavoritesRepository favoritesRepository, IImageServiceClient imageServiceClient)
    {
        _artworkRepository = artworkRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _favoritesRepository = favoritesRepository;
        _imageServiceClient = imageServiceClient;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ArtworkResponseDTO>> GetAllAsync()
    {
        var artworks = await _artworkRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task AddAsync(ArtworkRequestDTO artworkDto, IFormFile artworkImage)
    {
        if (artworkDto == null)
            throw new BadRequestException("Artwork parameters not provided correctly.");
        if (artworkImage == null || artworkImage.Length == 0)
            throw new BadRequestException("Image not provided correctly.");

        var artwork = _mapper.Map<Artwork>(artworkDto);
        artwork.ImageId =
            "pending"; // Temporary value to reserve the image slot until we get the real ID from the image service

        await _artworkRepository.AddAsync(artwork);
        await _artworkRepository.SaveAsync();

        try
        {
            var imageResult = await _imageServiceClient.UploadArtworkImageAsync(
                artwork.Id,
                new StreamPart(artworkImage.OpenReadStream(), artworkImage.FileName, artworkImage.ContentType)
            );

            artwork.ImageId = imageResult.ImageId.ToString();
            artwork.Color = imageResult.DominantColor;
            await _artworkRepository.SaveAsync();
        }
        catch
        {
            await _artworkRepository.DeleteAsync(artwork.Id);
            await _artworkRepository.SaveAsync();
            throw new BadRequestException("Failed to upload image. Artwork was not created.");
        }
    }

    /// <inheritdoc />
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
            var filePart = new StreamPart(artworkImage.OpenReadStream(), artworkImage.FileName,
                artworkImage.ContentType);

            var imageResult = string.IsNullOrEmpty(artwork.ImageId)
                ? await _imageServiceClient.UploadArtworkImageAsync(artwork.Id, filePart)
                : await _imageServiceClient.ReplaceArtworkImageAsync(artwork.ImageId, filePart);

            artwork.ImageId = imageResult.ImageId.ToString();
            artwork.Color = imageResult.DominantColor;
        }

        _artworkRepository.Update(artwork);
        await _artworkRepository.SaveAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        await _artworkRepository.DeleteAsync(id);
        await _artworkRepository.SaveAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ArtworkSearchResponseDTO>?> SearchByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new BadRequestException("Title parameter is required.");
        var artworks = await _artworkRepository.SearchByTitle(title);
        if (artworks == null || !artworks.Any())
            throw new NotFoundException($"No artworks found with this title.");
        return _mapper.Map<IEnumerable<ArtworkSearchResponseDTO>>(artworks);
    }

    /// <inheritdoc />
    public async Task ChangeVisibilityAsync(int id, bool isPrivate)
    {
        var artwork = await _artworkRepository.GetByIdAsync(id);
        if (artwork == null)
            throw new NotFoundException($"Artwork with id {id} not found.");
        artwork.ChangeVisibility(isPrivate);
        _artworkRepository.UpdateIsPrivate(artwork);
        await _artworkRepository.SaveAsync();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<IEnumerable<DiscoverArtworkDTO>?> GetDiscoverArtworksAsync(int loggedInUserId, int skip, int take)
    {
        var artworks = await _artworkRepository.GetDiscoverArtworksAsync(loggedInUserId, skip, take);
        return _mapper.Map<IEnumerable<DiscoverArtworkDTO>>(artworks);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<OnSaleArtworkDTO>?> GetOnSaleArtworksAsync(int skip, int take)
    {
        var artworks = await _artworkRepository.GetOnSaleArtworksAsync(skip, take);
        return _mapper.Map<IEnumerable<OnSaleArtworkDTO>>(artworks);
    }
}