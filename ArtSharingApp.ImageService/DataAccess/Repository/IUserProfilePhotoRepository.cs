using ArtSharingApp.ImageService.Models;

namespace ArtSharingApp.ImageService.DataAccess.Repository;

public interface IUserProfilePhotoRepository
{
    Task<UserProfilePhoto?> GetByIdAsync(Guid id);
    Task<UserProfilePhoto?> GetByUserRefIdAsync(int userRefId);
    Task AddAsync(UserProfilePhoto photo);
    void Delete(UserProfilePhoto photo);
    Task SaveChangesAsync();
}