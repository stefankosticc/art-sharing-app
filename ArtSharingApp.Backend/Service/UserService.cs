using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<User> GetUsersByName(string name)
    {
        return _userRepository.GetUsersByName(name);
    }

    public void AddUser(User user)
    {
        _userRepository.Add(user);
        _userRepository.Save();
    }

    public object? GetUserById(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
            return null;
        return user;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAll();
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

