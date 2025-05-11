using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IRoleService
{
    void AddRole(Role role);
    IEnumerable<Role> GetAll();
    Role? GetById(int id);
    void Update(int id, Role role);
    void Delete(int id);
}
