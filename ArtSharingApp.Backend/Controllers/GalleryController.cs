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
    public IActionResult GetAll()
    {
        return Ok(_galleryService.GetAll());
    }

    [HttpGet("gallery/{id}")]
    public IActionResult GetById(int id)
    {
        var gallery = _galleryService.GetById(id);
        if (gallery == null)
            return NotFound();
        return Ok(gallery);
    }

    [HttpPost("gallery")]
    public IActionResult Add([FromBody] GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null)
            return BadRequest("Gallery object is null.");
        _galleryService.Add(galleryDto);
        return Ok();
    }

    [HttpPut("gallery/{id}")]
    public IActionResult Update(int id, [FromBody] GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null)
            return BadRequest("Gallery object is null.");
        if (_galleryService.GetById(id) == null)
            return NotFound();
        _galleryService.Update(id, galleryDto);
        return Ok();
    }

    [HttpDelete("gallery/{id}")]
    public IActionResult Delete(int id)
    {
        if (_galleryService.GetById(id) == null)
            return NotFound();
        _galleryService.Delete(id);
        return Ok();
    }
}
