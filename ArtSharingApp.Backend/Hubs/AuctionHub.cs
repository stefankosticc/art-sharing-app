using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ArtSharingApp.Backend.Hubs;

[Authorize]
public class AuctionHub : Hub
{
    public async Task JoinAuction(int auctionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"auction-{auctionId}");
    }

    public async Task LeaveAuction(int auctionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"auction-{auctionId}");
    }
}