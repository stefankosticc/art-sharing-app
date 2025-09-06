using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Models;
using System.Collections.Generic;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing roles.
/// </summary>
public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    /// <param name="roleRepository">Repository for role data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    public RoleService(IGenericRepository<Role> roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Adds a new role.
    /// </summary>
    /// <param name="roleDto">The role data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    public async Task AddRoleAsync(RoleRequestDTO roleDto)
    {
        if (roleDto == null || string.IsNullOrWhiteSpace(roleDto.Name))
            throw new BadRequestException("Role parameters not provided correctly.");
        var role = _mapper.Map<Role>(roleDto);
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveAsync();
    }

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    /// <returns>A collection of <see cref="RoleResponseDTO"/> representing all roles.</returns>
    public async Task<IEnumerable<RoleResponseDTO>> GetAllAsync()
    {
        IEnumerable<Role> roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleResponseDTO>>(roles);
    }

    /// <summary>
    /// Retrieves a role by its ID.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <returns>The <see cref="RoleResponseDTO"/> for the specified role.</returns>
    /// <exception cref="NotFoundException">Thrown if the role is not found.</exception>
    public async Task<RoleResponseDTO?> GetByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException($"Role with id {id} not found.");
        return _mapper.Map<RoleResponseDTO>(role);
    }

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <param name="roleDto">The updated role data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the role is not found.</exception>
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

    /// <summary>
    /// Deletes a role by its ID.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <exception cref="NotFoundException">Thrown if the role is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException($"Role with id {id} not found.");
        await _roleRepository.DeleteAsync(id);
        await _roleRepository.SaveAsync();
    }
}
