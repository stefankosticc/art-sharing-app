using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IRoleService
{
    void AddRole(RoleRequestDTO role);
    IEnumerable<RoleResponseDTO> GetAll();
    RoleResponseDTO? GetById(int id);
    void Update(int id, RoleRequestDTO role);
    void Delete(int id);
}
