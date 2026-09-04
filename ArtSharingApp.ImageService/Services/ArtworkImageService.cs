using ArtSharingApp.ImageService.DataAccess.Repository;
using ArtSharingApp.ImageService.DTO;
using ArtSharingApp.ImageService.Exceptions;
using ArtSharingApp.ImageService.Models;
using ArtSharingApp.ImageService.Utils;
using AutoMapper;

namespace ArtSharingApp.ImageService.Services;

public class ArtworkImageService : IArtworkImageService
{
    private readonly IArtworkImageRepository _artworkImageRepository;
    private readonly IMapper _mapper;

    public ArtworkImageService(IArtworkImageRepository artworkImageRepository, IMapper mapper)
    {
        _artworkImageRepository = artworkImageRepository;
        _mapper = mapper;
    }

    private static string? TryExtractColor(byte[] imageBytes)
    {
        try
        {
            return ImageColorHelper.ExtractSaturationWeightedAverageColor(imageBytes);
        }
        catch
        {
            return null;
        }
    }

    public async Task<ArtworkImageDTO> UploadAsync(int artworkRefId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new BadRequestException("Image not provided correctly.");

        var imageBytes = await FileHelper.ReadFileBytesAsync(file);
        var color = TryExtractColor(imageBytes);

        var image = ArtworkImage.Create(artworkRefId, imageBytes, file.ContentType);
        await _artworkImageRepository.AddAsync(image);
        await _artworkImageRepository.SaveChangesAsync();

        var dto = _mapper.Map<ArtworkImageDTO>(image);
        dto.DominantColor = color;
        return dto;
    }

    public async Task<(byte[] Data, string ContentType)> GetByIdAsync(Guid imageId)
    {
        var image = await _artworkImageRepository.GetByIdAsync(imageId);
        if (image?.ImageData == null || image.ImageData.Length == 0)
            throw new NotFoundException("Image not found.");

        return (image.ImageData, string.IsNullOrWhiteSpace(image.ContentType) ? "image/jpeg" : image.ContentType);
    }

    public async Task<ArtworkImageDTO> ReplaceAsync(Guid imageId, IFormFile file)
    {
        var image = await _artworkImageRepository.GetByIdAsync(imageId)
                    ?? throw new NotFoundException($"Artwork image not found.");

        var imageBytes = await FileHelper.ReadFileBytesAsync(file);
        var color = TryExtractColor(imageBytes);

        image.Update(imageBytes, file.ContentType);
        await _artworkImageRepository.SaveChangesAsync();

        var dto = _mapper.Map<ArtworkImageDTO>(image);
        dto.DominantColor = color;
        return dto;
    }

    public async Task DeleteAsync(Guid imageId)
    {
        var image = await _artworkImageRepository.GetByIdAsync(imageId)
                    ?? throw new NotFoundException($"Artwork image not found.");

        _artworkImageRepository.Delete(image);
        await _artworkImageRepository.SaveChangesAsync();
    }
}