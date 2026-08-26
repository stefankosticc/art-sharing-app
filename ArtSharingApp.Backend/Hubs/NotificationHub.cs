using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ArtSharingApp.Backend.Hubs;

[Authorize]
public class NotificationHub : Hub
{
}