using ArtSharingApp.ImageService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.ImageService.Controllers;

[ApiController]
[Route("images/artworks")]
public class ArtworkImageController : ControllerBase
{
    private readonly IArtworkImageService _artworkImageService;

    public ArtworkImageController(IArtworkImageService artworkImageService)
    {
        _artworkImageService = artworkImageService;
    }

    [HttpPost]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> Upload([FromQuery] int artworkRefId, IFormFile image)
    {
        var result = await _artworkImageService.UploadAsync(artworkRefId, image);
        return Ok(result);
    }

    [HttpGet("{imageId:guid}")]
    public async Task<IActionResult> Get(Guid imageId)
    {
        var (data, contentType) = await _artworkImageService.GetByIdAsync(imageId);
        return File(data, contentType);
    }

    [HttpPut("{imageId:guid}")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> Replace(Guid imageId, IFormFile file)
    {
        var result = await _artworkImageService.ReplaceAsync(imageId, file);
        return Ok(result);
    }

    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> Delete(Guid imageId)
    {
        await _artworkImageService.DeleteAsync(imageId);
        return Ok(new { message = "Artwork image deleted successfully." });
    }
}