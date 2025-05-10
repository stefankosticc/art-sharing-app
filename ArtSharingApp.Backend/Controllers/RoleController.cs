using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
    public IActionResult AddRole([FromBody] Role role)
    {
        _roleService.AddRole(role);
        return Ok();
    }

    [HttpGet("roles")]
    public IActionResult GetAll()
    {
        IEnumerable<Role> roles = _roleService.GetAll();
        return Ok(roles);
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
    public IActionResult Update(int id, [FromBody] Role role)
    {
        if (role == null)
            return BadRequest("Role object is null.");
        var existing = _roleService.GetById(id);
        if (existing == null)
            return NotFound();
        _roleService.Update(id, role);
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
