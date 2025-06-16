using ArtSharingApp.Backend.Controllers.Common;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : AuthenticatedUserBaseController
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    
    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO request)
    {
        await _authService.Register(request);
        return Ok(new { message = "User registered successfully" });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
    {
        var result = await _authService.Login(loginDto);
        if (result != null)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    
    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
    {
        var response = await _authService.RefreshTokenAsync(request);
        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync(User);
        return Ok(new { message = "User logged out successfully" });
    }
    
    [Authorize]
    [HttpGet("loggedin-user")]
    public async Task<IActionResult> GetLoggedInUser()
    {
        var loggedInUserId = GetLoggedInUserId();
        var loggedInUser = await _userService.GetUserByIdAsync(loggedInUserId);
        return Ok(loggedInUser);
    }
}
