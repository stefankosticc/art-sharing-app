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
    public IActionResult GetAll()
    {
        return Ok(_artworkService.GetAll());
    }

    [HttpGet("artwork/{id}")]
    public IActionResult GetById(int id)
    {
        var artwork = _artworkService.GetById(id);
        if (artwork == null)
            return NotFound();
        return Ok(artwork);
    }

    [HttpPost("artwork")]
    public IActionResult Add([FromBody] ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            return BadRequest("Artwork object is null.");
        _artworkService.Add(artworkDto);
        return Ok();
    }

    [HttpPut("artwork/{id}")]
    public IActionResult Update(int id, [FromBody] ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            return BadRequest("Artwork object is null.");
        if (_artworkService.GetById(id) == null)
            return NotFound();
        _artworkService.Update(id, artworkDto);
        return Ok();
    }

    [HttpDelete("artwork/{id}")]
    public IActionResult Delete(int id)
    {
        if (_artworkService.GetById(id) == null)
            return NotFound();
        _artworkService.Delete(id);
        return Ok();
    }
}
