using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IRoleService
{
    Task AddRoleAsync(RoleRequestDTO role);
    Task<IEnumerable<RoleResponseDTO>> GetAllAsync();
    Task<RoleResponseDTO?> GetByIdAsync(int id);
    Task UpdateAsync(int id, RoleRequestDTO role);
    Task DeleteAsync(int id);
}
