using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class GalleryController : Controller
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    [HttpGet("galleries")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _galleryService.GetAllAsync());
    }

    [HttpGet("gallery/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var gallery = await _galleryService.GetByIdAsync(id);
        if (gallery == null)
            return NotFound();
        return Ok(gallery);
    }

    [HttpPost("gallery")]
    public async Task<IActionResult> Add([FromBody] GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null)
            return BadRequest("Gallery object is null.");
        await _galleryService.AddAsync(galleryDto);
        return Ok();
    }

    [HttpPut("gallery/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null)
            return BadRequest("Gallery object is null.");
        if (await _galleryService.GetByIdAsync(id) == null)
            return NotFound();
        await _galleryService.UpdateAsync(id, galleryDto);
        return Ok();
    }

    [HttpDelete("gallery/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _galleryService.GetByIdAsync(id) == null)
            return NotFound();
        await _galleryService.DeleteAsync(id);
        return Ok();
    }
    
    [HttpGet("gallery/{id}/artworks")]
    public async Task<IActionResult> GetArtworksByGalleryId(int id)
    {
        var artworks = await _galleryService.GetArtworksByGalleryId(id);
        if (artworks == null)
            return NotFound();
        return Ok(artworks);
    }
    
    [HttpGet("galleries/search")]
    public async Task<IActionResult> GetGalleriesByName([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name parameter is required.");
        return Ok(await _galleryService.GetGalleriesByName(name));
    }
}
