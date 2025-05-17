using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
}