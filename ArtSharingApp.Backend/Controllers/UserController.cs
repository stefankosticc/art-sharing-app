using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;

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
        await _userService.AddUserAsync(user);
        return Ok(new {message = "User added successfully"});
    }
    
    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteAsync(id);
        return Ok(new {message = "User deleted successfully"});
    }

    [HttpGet("users/by-name")]
    public async Task<IActionResult> GetUsersByName([FromQuery] string name)
    {
        var users = await _userService.GetUsersByName(name);
        return Ok(users);
    }
}
