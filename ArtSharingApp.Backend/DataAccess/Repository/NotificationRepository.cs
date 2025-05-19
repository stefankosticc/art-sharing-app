using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Notification>?> GetAllReadAndUnreadNotificationsAsync(int loggedInUserId)
    {
        return await _dbSet
            .Where(n => n.RecipientId == loggedInUserId && n.Status != NotificationStatus.DELETED)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public void UpdateNotificationStatus(Notification notification)
    {
        _context.Attach(notification);
        _context.Entry(notification).Property(n => n.Status).IsModified = true;
    }
}