using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("user/{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }

    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        return Ok(_userService.GetAllUsers());
    }
    
    [HttpPost("user")]
    public IActionResult AddUser([FromBody] UserRequestDTO user)
    {
        if (user == null)
            return BadRequest("User object is null.");
        
        _userService.AddUser(user);
        return Ok();
    }
    
    [HttpDelete("user/{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
            return NotFound();
        
        _userService.Delete(id);
        return Ok();
    }

    [HttpGet("users/by-name")]
    public IActionResult GetUsersByName([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name parameter is required.");
        return Ok(_userService.GetUsersByName(name));
    }
}
