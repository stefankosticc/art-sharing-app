using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Models;
using System.Collections.Generic;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IMapper _mapper;
    
    public RoleService(IGenericRepository<Role> roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public void AddRole(RoleRequestDTO roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);
        _roleRepository.Add(role);
        _roleRepository.Save();
    }

    public IEnumerable<RoleResponseDTO> GetAll()
    {
        IEnumerable<Role> roles = _roleRepository.GetAll();
        return _mapper.Map<IEnumerable<RoleResponseDTO>>(roles);
    }

    public RoleResponseDTO? GetById(int id)
    {
        var role = _roleRepository.GetById(id);
        if (role == null)
            return null;
        return _mapper.Map<RoleResponseDTO>(role);
    }

    public void Update(int id, RoleRequestDTO roleDto)
    {
        if (_roleRepository.GetById(id) == null) return;
        var role = _mapper.Map<Role>(roleDto);
        role.Id = id;
        _roleRepository.Update(role);
        _roleRepository.Save();
    }

    public void Delete(int id)
    {
        var role = _roleRepository.GetById(id);
        if (role == null) return;
        _roleRepository.Delete(role);
        _roleRepository.Save();
    }
}
