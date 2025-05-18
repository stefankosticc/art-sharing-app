using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IFollowersRepository : IGenericRepository<Followers>
{
    Task<bool> IsFollowing(int loggedInUserId, int userId);
    Task DeleteAsync(int loggedInUserId, int userId);
    Task<IEnumerable<Followers>> GetFollowersAsync(int loggedInUserId);
    Task<IEnumerable<Followers>> GetFollowingAsync(int loggedInUserId);
}