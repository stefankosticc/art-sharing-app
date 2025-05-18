using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IFollowersService
{
    Task<bool> FollowUserAsync(int loggedInUserId, int userId);
    Task<bool> UnfollowUserAsync(int loggedInUserId, int userId);
    Task<IEnumerable<FollowersDTO>?> GetFollowersAsync(int loggedInUserId);
    Task<IEnumerable<FollowingDTO>?> GetFollowingAsync(int loggedInUserId);
}