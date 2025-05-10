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
}

