using ArtSharingApp.ImageService.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.ImageService.DataAccess.Repository;

public class UserProfilePhotoRepository : IUserProfilePhotoRepository
{
    private readonly ImageDbContext _context;
    private readonly DbSet<UserProfilePhoto> _dbSet;

    public UserProfilePhotoRepository(ImageDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<UserProfilePhoto>();
    }

    public async Task<UserProfilePhoto?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task<UserProfilePhoto?> GetByUserRefIdAsync(int userRefId)
        => await _dbSet.FirstOrDefaultAsync(p => p.UserRefId == userRefId);

    public async Task AddAsync(UserProfilePhoto photo)
        => await _dbSet.AddAsync(photo);

    public void Delete(UserProfilePhoto photo)
        => _dbSet.Remove(photo);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}