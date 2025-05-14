using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Models;
using System.Collections.Generic;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;

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

    public async Task AddRoleAsync(RoleRequestDTO roleDto)
    {
        if (roleDto == null || string.IsNullOrWhiteSpace(roleDto.Name))
            throw new BadRequestException("Role parameters not provided correctly.");
        var role = _mapper.Map<Role>(roleDto);
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveAsync();
    }

    public async Task<IEnumerable<RoleResponseDTO>> GetAllAsync()
    {
        IEnumerable<Role> roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleResponseDTO>>(roles);
    }

    public async Task<RoleResponseDTO?> GetByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException($"Role with id {id} not found.");
        return _mapper.Map<RoleResponseDTO>(role);
    }

    public async Task UpdateAsync(int id, RoleRequestDTO roleDto)
    {
        if (roleDto == null || string.IsNullOrWhiteSpace(roleDto.Name))
            throw new BadRequestException("Role parameters not provided correctly.");
        
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException($"Role with id {id} not found.");
        _mapper.Map(roleDto, role);
        _roleRepository.Update(role);
        await _roleRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException($"Role with id {id} not found.");
        await _roleRepository.DeleteAsync(id);
        await _roleRepository.SaveAsync();
    }
}
