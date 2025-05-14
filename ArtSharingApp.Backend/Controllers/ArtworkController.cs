using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class ArtworkController : Controller
{
    private readonly IArtworkService _artworkService;

    public ArtworkController(IArtworkService artworkService)
    {
        _artworkService = artworkService;
    }

    [HttpGet("artworks")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _artworkService.GetAllAsync());
    }

    [HttpGet("artwork/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var artwork = await _artworkService.GetByIdAsync(id);
        return Ok(artwork);
    }

    [HttpPost("artwork")]
    public async Task<IActionResult> Add([FromBody] ArtworkRequestDTO artworkDto)
    {
        await _artworkService.AddAsync(artworkDto);
        return Ok(new {message = "Artwork added successfully."});
    }

    [HttpPut("artwork/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ArtworkRequestDTO artworkDto)
    {
        await _artworkService.UpdateAsync(id, artworkDto);
        return Ok(new {message = "Artwork updated successfully."});
    }

    [HttpDelete("artwork/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _artworkService.DeleteAsync(id);
        return Ok(new {message = "Artwork deleted successfully."});
    }
    
    [HttpGet("artworks/search")]
    public async Task<IActionResult> Search([FromQuery] string title)
    {
        var artworks = await _artworkService.SearchByTitle(title);
        return Ok(artworks);
    }
}
