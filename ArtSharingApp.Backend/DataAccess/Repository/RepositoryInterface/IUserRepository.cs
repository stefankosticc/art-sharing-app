using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IEnumerable<User>> GetUsersByName(string name);
    void UpdateBiography(User user);
}
