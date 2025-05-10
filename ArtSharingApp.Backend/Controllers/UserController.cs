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
    
    [HttpPost("user")]
    public IActionResult AddUser([FromBody] User user)
    {
        if (user == null)
            return BadRequest("User object is null.");
        
        _userService.AddUser(user);
        return Ok();
    }

    [HttpGet("users/by-name")]
    public IActionResult GetUsersByName([FromQuery] string name)
    {
        // if (string.IsNullOrWhiteSpace(name))
        //     return BadRequest("Name parameter is required.");
        //
        // var users = _userService.GetUsersByName(name);
        return Ok();
    }
}

