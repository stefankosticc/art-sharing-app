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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _cityService.GetAllAsync());
    }

    [HttpGet("city/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var city = await _cityService.GetByIdAsync(id);
        if (city == null)
            return NotFound();
        return Ok(city);
    }

    [HttpPost("city")]
    public async Task<IActionResult> Add([FromBody] CityRequestDTO cityDto)
    {
        if (cityDto == null)
            return BadRequest("City object is null.");
        await _cityService.AddAsync(cityDto);
        return Ok();
    }

    [HttpPut("city/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CityRequestDTO cityDto)
    {
        if (cityDto == null)
            return BadRequest("City object is null.");
        if (await _cityService.GetByIdAsync(id) == null)
            return NotFound();
        await _cityService.UpdateAsync(id, cityDto);
        return Ok();
    }

    [HttpDelete("city/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _cityService.GetByIdAsync(id) == null)
            return NotFound();
        await _cityService.DeleteAsync(id);
        return Ok();
    }
}
