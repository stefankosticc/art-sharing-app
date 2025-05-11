using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class CityController : Controller
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet("cities")]
    public IActionResult GetAll()
    {
        return Ok(_cityService.GetAll());
    }

    [HttpGet("city/{id}")]
    public IActionResult GetById(int id)
    {
        var city = _cityService.GetById(id);
        if (city == null)
            return NotFound();
        return Ok(city);
    }

    [HttpPost("city")]
    public IActionResult Add([FromBody] CityRequestDTO cityDto)
    {
        if (cityDto == null)
            return BadRequest("City object is null.");
        _cityService.Add(cityDto);
        return Ok();
    }

    [HttpPut("city/{id}")]
    public IActionResult Update(int id, [FromBody] CityRequestDTO cityDto)
    {
        if (cityDto == null)
            return BadRequest("City object is null.");
        if (_cityService.GetById(id) == null)
            return NotFound();
        _cityService.Update(id, cityDto);
        return Ok();
    }

    [HttpDelete("city/{id}")]
    public IActionResult Delete(int id)
    {
        if (_cityService.GetById(id) == null)
            return NotFound();
        _cityService.Delete(id);
        return Ok();
    }
}
