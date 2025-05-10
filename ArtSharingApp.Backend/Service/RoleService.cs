using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Models;
using System.Collections.Generic;

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

    public IEnumerable<Role> GetAll()
    {
        return _genericRepository.GetAll();
    }

    public Role? GetById(int id)
    {
        return _genericRepository.GetById(id);
    }

    public void Update(int id, Role role)
    {
        if (_genericRepository.GetById(id) == null) return;
        _genericRepository.Update(role);
        _genericRepository.Save();
    }

    public void Delete(int id)
    {
        var role = _genericRepository.GetById(id);
        if (role == null) return;
        _genericRepository.Delete(role);
        _genericRepository.Save();
    }
}
