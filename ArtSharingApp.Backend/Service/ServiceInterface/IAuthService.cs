using System.Security.Claims;
using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IAuthService
{
    Task Register(UserRegisterDTO request);
    Task<TokenResponseDTO?> Login(UserLoginDTO request);
    Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);
    Task LogoutAsync(ClaimsPrincipal userPrincipal);
}