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
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
    
    [HttpPost("user")]
    public async Task<IActionResult> AddUser([FromBody] UserRequestDTO user)
    {
        if (user == null)
            return BadRequest("User object is null.");
        
        await _userService.AddUserAsync(user);
        return Ok();
    }
    
    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        
        await _userService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("users/by-name")]
    public async Task<IActionResult> GetUsersByName([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name parameter is required.");
        var users = await _userService.GetUsersByName(name);
        return Ok(users);
    }
}
