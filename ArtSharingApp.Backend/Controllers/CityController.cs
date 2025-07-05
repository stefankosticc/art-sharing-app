using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
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
        return Ok(city);
    }

    [HttpPost("city")]
    public async Task<IActionResult> Add([FromBody] CityRequestDTO cityDto)
    {
        await _cityService.AddAsync(cityDto);
        return Ok(new {message = "City added successfully."});
    }

    [HttpPut("city/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CityRequestDTO cityDto)
    {
        await _cityService.UpdateAsync(id, cityDto);
        return Ok(new {message = "City updated successfully."});
    }

    [HttpDelete("city/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cityService.DeleteAsync(id);
        return Ok(new {message = "City deleted successfully."});
    }
    
    [HttpGet("cities/search")]
    public async Task<IActionResult> GetGalleriesByName([FromQuery] string name)
    {
        return Ok(await _cityService.GetCitiesByName(name));
    }
}
