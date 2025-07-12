using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task AddUserAsync(UserRequestDTO userDto)
    {
        if (userDto == null)
            throw new BadRequestException("User parameters not provided correctly.");
        var user = _mapper.Map<User>(userDto);
        await _userRepository.AddAsync(user); 
        await _userRepository.SaveAsync();
    }

    public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} not found.");
        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} not found.");
        await _userRepository.DeleteAsync(id);
        await _userRepository.SaveAsync();
    }

    public async Task UpdateUserBiographyAsync(int userId, string biography)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException($"User with id {userId} not found.");
        user.Biography = biography;
        _userRepository.UpdateBiography(user);
        await _userRepository.SaveAsync();
    }

    public async Task<IEnumerable<UserSearchResponseDTO?>> GetUsersByNameAndUserName(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            throw new BadRequestException("Username parameter is required.");
        var users = await _userRepository.GetUsersByNameAndUserName(searchString);
        return _mapper.Map<IEnumerable<UserSearchResponseDTO>>(users);
    }
}
