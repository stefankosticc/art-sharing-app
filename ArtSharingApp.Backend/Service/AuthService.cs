using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ArtSharingApp.Backend.Service;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    
    public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }
    
    public async Task Register(UserRegisterDTO request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new BadRequestException("User already exists");

        var role = await _roleManager.FindByNameAsync("User");
        if (role == null)
            throw new NotFoundException("Role not found");
        
        var user = new User()
        {
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            RoleId = role.Id
        };
        
        // Create user
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"User registration failed: {errorMessages}");
        }
        
        // Assign role to user
        var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
        if (!roleResult.Succeeded)
            throw new BadRequestException("Role assignment failed");
    }

    public async Task<TokenResponseDTO?> Login(UserLoginDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new BadRequestException("Invalid email or password");

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
            throw new BadRequestException("Invalid email or password");
        
        var response = new TokenResponseDTO()
        {
            AccessToken = await GenerateJwtToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
        return response;
    }

    public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        if (user == null || string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiresAt <= DateTime.UtcNow)
            throw new BadRequestException("Invalid refresh token");
        
        var newRefreshToken = await GenerateAndSaveRefreshTokenAsync(user);
        var newAccessToken = await GenerateJwtToken(user);
        
        return new TokenResponseDTO()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task LogoutAsync(ClaimsPrincipal userPrincipal)
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new BadRequestException("User not found");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");
        
        if (user.RefreshToken == null)
            throw new BadRequestException("User is not logged in");
        
        // Invalidate the refresh token by setting it to null
        user.RefreshToken = null; 
        user.RefreshTokenExpiresAt = null; 
        await _userManager.UpdateAsync(user);
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var role = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role.FirstOrDefault() ?? string.Empty)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);
        return refreshToken;
    }
    
}
