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

    public IEnumerable<UserResponseDTO> GetUsersByName(string name)
    {
        var users = _userRepository.GetUsersByName(name);
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public void AddUser(UserRequestDTO userDto)
    {
        var user = _mapper.Map<User>(userDto);
        _userRepository.Add(user); 
        _userRepository.Save();
    }

    public object? GetUserById(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
            return null;
        return _mapper.Map<UserResponseDTO>(user);
    }

    public IEnumerable<UserResponseDTO> GetAllUsers()
    {
        IEnumerable<User> users = _userRepository.GetAll();
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public void Delete(int id)
    {
        var user = _userRepository.GetById(id);
        if (user != null)
        {
            _userRepository.Delete(id);
            _userRepository.Save();
        }
        else
        {
            throw new Exception("User not found");
        }
    }
}

