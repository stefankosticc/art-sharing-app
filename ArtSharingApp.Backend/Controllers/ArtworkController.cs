using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using System.Collections.Generic;

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
        IEnumerable<Artwork> artworks = _artworkService.GetAll();
        return Ok(artworks);
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
    public IActionResult Add([FromBody] Artwork artwork)
    {
        if (artwork == null)
            return BadRequest("Artwork object is null.");
        _artworkService.Add(artwork);
        return Ok();
    }

    [HttpPut("artwork/{id}")]
    public IActionResult Update(int id, [FromBody] Artwork artwork)
    {
        if (artwork == null)
            return BadRequest("Artwork object is null.");
        if (_artworkService.GetById(id) == null)
            return NotFound();
        _artworkService.Update(id, artwork);
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
