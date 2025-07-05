using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IUserRepository : IGenericRepository<User>
{
    void UpdateBiography(User user);
    Task<IEnumerable<User>> GetUsersByNameAndUserName(string searchString);
}
