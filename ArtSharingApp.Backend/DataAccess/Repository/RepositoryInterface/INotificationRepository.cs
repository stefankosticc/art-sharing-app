using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>?> GetAllReadAndUnreadNotificationsAsync(int loggedInUserId);
    void UpdateNotificationStatus(Notification notification);
}