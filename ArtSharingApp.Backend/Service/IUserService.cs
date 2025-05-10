using ArtSharingApp.Backend.Models;
using System.Collections.Generic;

namespace ArtSharingApp.Backend.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetUsersByName(string name);
        void AddUser(User user);
        object? GetUserById(int id);
        IEnumerable<User> GetAllUsers();
        void Delete(int id);
    }
}
