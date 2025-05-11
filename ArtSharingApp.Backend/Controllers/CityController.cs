using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using System.Collections.Generic;
using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class CityController : Controller
{
    private readonly ICityService _cityService;
    private readonly IMapper _mapper;

    public CityController(ICityService cityService, IMapper mapper)
    {
        _cityService = cityService;
        _mapper = mapper;
    }

    [HttpGet("cities")]
    public IActionResult GetAll()
    {
        IEnumerable<City> cities = _cityService.GetAll();
        var dtos = _mapper.Map<IEnumerable<CityResponseDTO>>(cities);
        return Ok(dtos);
    }

    [HttpGet("city/{id}")]
    public IActionResult GetById(int id)
    {
        var city = _cityService.GetById(id);
        if (city == null)
            return NotFound();
        var dto = _mapper.Map<CityResponseDTO>(city);
        return Ok(dto);
    }

    [HttpPost("city")]
    public IActionResult Add([FromBody] CityRequestDTO cityDto)
    {
        if (cityDto == null)
            return BadRequest("City object is null.");
        var city = _mapper.Map<City>(cityDto);
        _cityService.Add(city);
        return Ok();
    }

    [HttpPut("city/{id}")]
    public IActionResult Update(int id, [FromBody] CityRequestDTO cityDto)
    {
        if (cityDto == null)
            return BadRequest("City object is null.");
        var existing = _cityService.GetById(id);
        if (existing == null)
            return NotFound();
        var city = _mapper.Map<City>(cityDto);
        city.Id = id;
        _cityService.Update(id, city);
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
