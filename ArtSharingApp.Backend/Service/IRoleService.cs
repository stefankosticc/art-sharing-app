using ArtSharingApp.Backend.Models;
using System.Collections.Generic;

namespace ArtSharingApp.Backend.Service;

public interface IRoleService
{
    void AddRole(Role role);
    IEnumerable<Role> GetAll();
    Role? GetById(int id);
    void Update(int id, Role role);
    void Delete(int id);
}
