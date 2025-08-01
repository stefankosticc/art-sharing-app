using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Service;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public NotificationService(
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NotificationResponseDTO>?> GetNotificationsAsync(int loggedInUserId, int skip,
        int take)
    {
        var notifications =
            await _notificationRepository.GetAllReadAndUnreadNotificationsAsync(loggedInUserId, skip, take);
        return _mapper.Map<IEnumerable<NotificationResponseDTO>>(notifications);
    }

    public async Task CreateNotificationAsync(NotificationRequestDTO request)
    {
        if (string.IsNullOrEmpty(request.Text) || request.RecipientId <= 0)
            throw new BadRequestException("Invalid notification request.");

        if (await _userRepository.GetByIdAsync(request.RecipientId) == null)
            throw new NotFoundException("Recipient not found.");

        var notification = _mapper.Map<Notification>(request);
        notification.CreatedAt = DateTime.UtcNow;
        notification.Status = NotificationStatus.UNREAD;

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveAsync();
    }

    public async Task MarkNotificationAsReadAsync(int notificationId, int loggedInUserId)
    {
        await ChangeNotificationStatus(notificationId, loggedInUserId, NotificationStatus.READ);
    }

    public async Task MarkNotificationAsUnreadAsync(int notificationId, int loggedInUserId)
    {
        await ChangeNotificationStatus(notificationId, loggedInUserId, NotificationStatus.UNREAD);
    }

    public async Task MarkNotificationAsDeletedAsync(int notificationId, int loggedInUserId)
    {
        await ChangeNotificationStatus(notificationId, loggedInUserId, NotificationStatus.DELETED);
    }

    private async Task ChangeNotificationStatus(int notificationId, int loggedInUserId, NotificationStatus status)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null)
            throw new NotFoundException("Notification not found.");

        if (notification.RecipientId != loggedInUserId)
            switch (status)
            {
                case NotificationStatus.READ:
                    throw new UnauthorizedAccessException(
                        "You do not have permission to mark this notification as read.");
                case NotificationStatus.UNREAD:
                    throw new UnauthorizedAccessException(
                        "You do not have permission to mark this notification as unread.");
                case NotificationStatus.DELETED:
                    throw new UnauthorizedAccessException("You do not have permission to delete this notification.");
            }

        notification.ChangeStatus(status);
        _notificationRepository.UpdateNotificationStatus(notification);
        await _notificationRepository.SaveAsync();
    }
}