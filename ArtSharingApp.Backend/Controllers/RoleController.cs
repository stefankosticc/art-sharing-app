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
    private readonly IMapper _mapper;
    
    public RoleController(IRoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }
    
    [HttpPost("role")]
    public IActionResult AddRole([FromBody] RoleRequestDTO roleDto)
    {
        if (roleDto == null)
            return BadRequest("Role object is null.");
        var role = _mapper.Map<Role>(roleDto);
        _roleService.AddRole(role);
        return Ok();
    }

    [HttpGet("roles")]
    public IActionResult GetAll()
    {
        IEnumerable<Role> roles = _roleService.GetAll();
        var dtos = _mapper.Map<IEnumerable<RoleResponseDTO>>(roles);
        return Ok(dtos);
    }

    [HttpGet("role/{id}")]
    public IActionResult GetById(int id)
    {
        var role = _roleService.GetById(id);
        if (role == null)
            return NotFound();
        var dto = _mapper.Map<RoleResponseDTO>(role);
        return Ok(dto);
    }

    [HttpPut("role/{id}")]
    public IActionResult Update(int id, [FromBody] RoleRequestDTO roleDto)
    {
        if (roleDto == null)
            return BadRequest("Role object is null.");
        var existing = _roleService.GetById(id);
        if (existing == null)
            return NotFound();
        var role = _mapper.Map<Role>(roleDto);
        role.Id = id;
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
