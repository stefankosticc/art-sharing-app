using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class ArtworkController : Controller
{
    private readonly IArtworkService _artworkService;
    private readonly IMapper _mapper;

    public ArtworkController(IArtworkService artworkService, IMapper mapper)
    {
        _artworkService = artworkService;
        _mapper = mapper;
    }

    [HttpGet("artworks")]
    public IActionResult GetAll()
    {
        IEnumerable<Artwork> artworks = _artworkService.GetAll();
        var dtos = _mapper.Map<IEnumerable<ArtworkResponseDTO>>(artworks);
        return Ok(dtos);
    }

    [HttpGet("artwork/{id}")]
    public IActionResult GetById(int id)
    {
        var artwork = _artworkService.GetById(id);
        if (artwork == null)
            return NotFound();
        var dto = _mapper.Map<ArtworkResponseDTO>(artwork);
        return Ok(dto);
    }

    [HttpPost("artwork")]
    public IActionResult Add([FromBody] ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            return BadRequest("Artwork object is null.");
        var artwork = _mapper.Map<Artwork>(artworkDto);
        _artworkService.Add(artwork);

        // Fetch the saved artwork with navigation properties
        var savedArtwork = _artworkService.GetById(artwork.Id);
        var responseDto = _mapper.Map<ArtworkResponseDTO>(savedArtwork);
        return Ok(responseDto);
    }

    [HttpPut("artwork/{id}")]
    public IActionResult Update(int id, [FromBody] ArtworkRequestDTO artworkDto)
    {
        if (artworkDto == null)
            return BadRequest("Artwork object is null.");
        if (_artworkService.GetById(id) == null)
            return NotFound();
        var artwork = _mapper.Map<Artwork>(artworkDto);
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
