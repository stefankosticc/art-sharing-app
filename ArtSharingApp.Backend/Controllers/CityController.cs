using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using System.Collections.Generic;
using ArtSharingApp.Backend.DataAccess.Repository;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class CityController : Controller
{
    private readonly IGenericRepository<City> _cityService;

    public CityController(IGenericRepository<City> cityService)
    {
        _cityService = cityService;
    }

    [HttpGet("cities")]
    public IActionResult GetAll()
    {
        IEnumerable<City> cities = _cityService.GetAll();
        return Ok(cities);
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
    public IActionResult Add([FromBody] City city)
    {
        if (city == null)
            return BadRequest("City object is null.");
        _cityService.Add(city);
        return Ok();
    }

    [HttpPut("city/{id}")]
    public IActionResult Update(int id, [FromBody] City city)
    {
        if (city == null)
            return BadRequest("City object is null.");
        var existing = _cityService.GetById(id);
        if (existing == null)
            return NotFound();
        _cityService.Update(city);
        return Ok();
    }

    [HttpDelete("city/{id}")]
    public IActionResult Delete(int id)
    {
        var city = _cityService.GetById(id);
        if (city == null)
            return NotFound();
        _cityService.Delete(id);
        return Ok();
    }
}
