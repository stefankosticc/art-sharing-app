using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface
{
    public interface IUserService
    {
        Task AddUserAsync(UserRequestDTO user);
        Task<UserResponseDTO?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
        Task DeleteAsync(int id);
        Task UpdateUserBiographyAsync(int userId, string biography);
        Task<IEnumerable<UserSearchResponseDTO?>> GetUsersByNameAndUserName(string searchString);
    }
}
