using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

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
}