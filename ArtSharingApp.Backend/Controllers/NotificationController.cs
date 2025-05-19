using ArtSharingApp.Backend.Controllers.Common;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class NotificationController : AuthenticatedUserBaseController
{
    private readonly INotificationService _notificationService;
    
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = GetLoggedInUserId();
        var notifications = await _notificationService.GetNotificationsAsync(userId);
        return Ok(notifications);
    }

    [HttpPost("notification")]
    public async Task<IActionResult> CreateNotification([FromBody] NotificationRequestDTO request)
    {
        await _notificationService.CreateNotificationAsync(request);
        return Ok(new { message = "Notification sent successfully." });
    }

    [HttpPut("notification/{id}/read")]
    public async Task<IActionResult> MarkNotificationAsRead(int id)
    {
        var userId = GetLoggedInUserId();
        await _notificationService.MarkNotificationAsReadAsync(id, userId);
        return Ok(new { message = "Notification marked as read." });
    }
    
    [HttpPut("notification/{id}/unread")]
    public async Task<IActionResult> MarkNotificationAsUnead(int id)
    {
        var userId = GetLoggedInUserId();
        await _notificationService.MarkNotificationAsUneadAsync(id, userId);
        return Ok(new { message = "Notification marked as unread." });
    }
    
    [HttpPut("notification/{id}/delete")]
    public async Task<IActionResult> MarkNotificationAsDeleted(int id)
    {
        var userId = GetLoggedInUserId();
        await _notificationService.MarkNotificationAsDeletedAsync(id, userId);
        return Ok(new { message = "Notification deleted." });
    }
}