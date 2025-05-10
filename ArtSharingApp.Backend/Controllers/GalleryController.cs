using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using System.Collections.Generic;
using ArtSharingApp.Backend.DataAccess.Repository;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class GalleryController : Controller
{
    private readonly IGenericRepository<Gallery> _galleryService;

    public GalleryController(IGenericRepository<Gallery> galleryService)
    {
        _galleryService = galleryService;
    }

    [HttpGet("galleries")]
    public IActionResult GetAll()
    {
        IEnumerable<Gallery> galleries = _galleryService.GetAll();
        return Ok(galleries);
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
    public IActionResult Add([FromBody] Gallery gallery)
    {
        if (gallery == null)
            return BadRequest("Gallery object is null.");
        _galleryService.Add(gallery);
        return Ok();
    }

    [HttpPut("gallery/{id}")]
    public IActionResult Update(int id, [FromBody] Gallery gallery)
    {
        if (gallery == null)
            return BadRequest("Gallery object is null.");
        var existing = _galleryService.GetById(id);
        if (existing == null)
            return NotFound();
        _galleryService.Update(gallery);
        return Ok();
    }

    [HttpDelete("gallery/{id}")]
    public IActionResult Delete(int id)
    {
        var gallery = _galleryService.GetById(id);
        if (gallery == null)
            return NotFound();
        _galleryService.Delete(id);
        return Ok();
    }
}
