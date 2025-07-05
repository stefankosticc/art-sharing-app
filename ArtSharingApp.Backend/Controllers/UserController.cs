using ArtSharingApp.Backend.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class UserController : AuthenticatedUserBaseController
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
        return Ok(new { message = "User added successfully" });
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteAsync(id);
        return Ok(new { message = "User deleted successfully" });
    }

    [HttpGet("users/search")]
    public async Task<IActionResult> GetUsersByNameAndUserName([FromQuery] string searchString)
    {
        var users = await _userService.GetUsersByNameAndUserName(searchString);
        return Ok(users);
    }

    [HttpPut("user/biography")]
    public async Task<IActionResult> UpdateUserBiography([FromBody] UpdateUserBiographyRequestDTO request)
    {
        var loggedInUserId = GetLoggedInUserId();
        string biography = request.Biography;
        await _userService.UpdateUserBiographyAsync(loggedInUserId, biography);
        return Ok(new { message = "User biography updated successfully" });
    }
}
