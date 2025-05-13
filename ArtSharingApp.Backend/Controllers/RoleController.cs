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
    public async Task<IActionResult> AddRole([FromBody] RoleRequestDTO roleDto)
    {
        if (roleDto == null)
            return BadRequest("Role object is null.");
        await _roleService.AddRoleAsync(roleDto);
        return Ok();
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _roleService.GetAllAsync());
    }

    [HttpGet("role/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
            return NotFound();
        return Ok(role);
    }

    [HttpPut("role/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoleRequestDTO roleDto)
    {
        if (roleDto == null)
            return BadRequest("Role object is null.");
        var existing = await _roleService.GetByIdAsync(id);
        if (existing == null)
            return NotFound();
        await _roleService.UpdateAsync(id, roleDto);
        return Ok();
    }

    [HttpDelete("role/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
            return NotFound();
        await _roleService.DeleteAsync(id);
        return Ok();
    }
}
