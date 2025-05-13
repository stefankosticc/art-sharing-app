using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;

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

    public async Task<IEnumerable<UserResponseDTO>> GetUsersByName(string name)
    {
        var users = await _userRepository.GetUsersByName(name);
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public async Task AddUserAsync(UserRequestDTO userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userRepository.AddAsync(user); 
        await _userRepository.SaveAsync();
    }

    public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return null;
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
        if (user != null)
        {
            await _userRepository.DeleteAsync(id);
            await _userRepository.SaveAsync();
        }
        else
        {
            throw new Exception("User not found");
        }
    }
}
