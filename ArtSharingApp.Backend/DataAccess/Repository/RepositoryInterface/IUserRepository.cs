using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IUserRepository : IGenericRepository<User>
{
    IEnumerable<User> GetUsersByName(string name);
}
