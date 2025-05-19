using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface INotificationService
{
    Task<IEnumerable<Notification>?> GetNotificationsAsync(int loggedInUserId);
    Task CreateNotificationAsync(NotificationRequestDTO request);
    Task MarkNotificationAsReadAsync(int notificationId, int loggedInUserId);
    Task MarkNotificationAsUneadAsync(int notificationId, int loggedInUserId);
    Task MarkNotificationAsDeletedAsync(int notificationId, int loggedInUserId);
}