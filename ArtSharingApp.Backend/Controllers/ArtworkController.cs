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
        if (artwork == null)
            return NotFound();
        return Ok(artwork);
    }

    [HttpPost("artwork")]
    public async Task<IActionResult> Add([FromBody] ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            return BadRequest("Artwork object is null.");
        await _artworkService.AddAsync(artworkDto);
        return Ok();
    }

    [HttpPut("artwork/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            return BadRequest("Artwork object is null.");
        if (await _artworkService.GetByIdAsync(id) == null)
            return NotFound();
        await _artworkService.UpdateAsync(id, artworkDto);
        return Ok();
    }

    [HttpDelete("artwork/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _artworkService.GetByIdAsync(id) == null)
            return NotFound();
        await _artworkService.DeleteAsync(id);
        return Ok();
    }
    
    [HttpGet("artworks/search")]
    public async Task<IActionResult> Search([FromQuery] string title)
    {
        var artworks = await _artworkService.SearchByTitle(title);
        if (artworks == null || !artworks.Any())
            return NotFound();
        return Ok(artworks);
    }
}
