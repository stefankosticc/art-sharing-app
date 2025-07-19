using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponseDTO>?> GetNotificationsAsync(int loggedInUserId, int skip = 0, int take = 10);
    Task CreateNotificationAsync(NotificationRequestDTO request);
    Task MarkNotificationAsReadAsync(int notificationId, int loggedInUserId);
    Task MarkNotificationAsUnreadAsync(int notificationId, int loggedInUserId);
    Task MarkNotificationAsDeletedAsync(int notificationId, int loggedInUserId);
}