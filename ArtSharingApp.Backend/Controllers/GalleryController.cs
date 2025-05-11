using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class GalleryController : Controller
{
    private readonly IGenericRepository<Gallery> _galleryService;
    private readonly IMapper _mapper;

    public GalleryController(IGenericRepository<Gallery> galleryService, IMapper mapper)
    {
        _galleryService = galleryService;
        _mapper = mapper;
    }

    [HttpGet("galleries")]
    public IActionResult GetAll()
    {
        IEnumerable<Gallery> galleries = _galleryService.GetAll();
        var dtos = _mapper.Map<IEnumerable<GalleryResponseDTO>>(galleries);
        return Ok(dtos);
    }

    [HttpGet("gallery/{id}")]
    public IActionResult GetById(int id)
    {
        var gallery = _galleryService.GetById(id);
        if (gallery == null)
            return NotFound();
        var dto = _mapper.Map<GalleryResponseDTO>(gallery);
        return Ok(dto);
    }

    [HttpPost("gallery")]
    public IActionResult Add([FromBody] GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null)
            return BadRequest("Gallery object is null.");
        var gallery = _mapper.Map<Gallery>(galleryDto);
        _galleryService.Add(gallery);
        return Ok();
    }

    [HttpPut("gallery/{id}")]
    public IActionResult Update(int id, [FromBody] GalleryRequestDTO galleryDto)
    {
        if (galleryDto == null)
            return BadRequest("Gallery object is null.");
        var existing = _galleryService.GetById(id);
        if (existing == null)
            return NotFound();
        var gallery = _mapper.Map<Gallery>(galleryDto);
        gallery.Id = id;
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
