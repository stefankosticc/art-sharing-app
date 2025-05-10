using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public interface IUserRepository : IGenericRepository<User>
{
    IEnumerable<User> GetUsersByName(string name);
}
