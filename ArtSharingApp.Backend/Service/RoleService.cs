using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _genericRepository;
    
    public RoleService(IGenericRepository<Role> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public void AddRole(Role role)
    {
        _genericRepository.Add(role);
        _genericRepository.Save();
    }
}
