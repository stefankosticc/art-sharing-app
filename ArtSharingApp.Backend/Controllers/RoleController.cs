using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpPost("role")]
    public IActionResult AddRole([FromBody] RoleRequestDTO roleDto)
    {
        if (roleDto == null)
            return BadRequest("Role object is null.");
        _roleService.AddRole(roleDto);
        return Ok();
    }

    [HttpGet("roles")]
    public IActionResult GetAll()
    {
        return Ok(_roleService.GetAll());
    }

    [HttpGet("role/{id}")]
    public IActionResult GetById(int id)
    {
        var role = _roleService.GetById(id);
        if (role == null)
            return NotFound();
        return Ok(role);
    }

    [HttpPut("role/{id}")]
    public IActionResult Update(int id, [FromBody] RoleRequestDTO roleDto)
    {
        if (roleDto == null)
            return BadRequest("Role object is null.");
        var existing = _roleService.GetById(id);
        if (existing == null)
            return NotFound();
        _roleService.Update(id, roleDto);
        return Ok();
    }

    [HttpDelete("role/{id}")]
    public IActionResult Delete(int id)
    {
        var role = _roleService.GetById(id);
        if (role == null)
            return NotFound();
        _roleService.Delete(id);
        return Ok();
    }
}
