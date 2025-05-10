using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        IEnumerable<User> users = _userService.GetAllUsers();
        return Ok(users);
    }
    
    [HttpPost("user")]
    public IActionResult AddUser([FromBody] User user)
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
        
        var users = _userService.GetUsersByName(name);
        return Ok();
    }
}

