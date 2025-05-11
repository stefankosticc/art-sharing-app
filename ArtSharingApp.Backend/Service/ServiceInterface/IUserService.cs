using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service.ServiceInterface
{
    public interface IUserService
    {
        IEnumerable<UserResponseDTO> GetUsersByName(string name);
        void AddUser(UserRequestDTO user);
        object? GetUserById(int id);
        IEnumerable<UserResponseDTO> GetAllUsers();
        void Delete(int id);
    }
}
