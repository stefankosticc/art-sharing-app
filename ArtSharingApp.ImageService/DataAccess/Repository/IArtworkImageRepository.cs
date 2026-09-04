using ArtSharingApp.ImageService.Models;

namespace ArtSharingApp.ImageService.DataAccess.Repository;

public interface IArtworkImageRepository
{
    Task<ArtworkImage?> GetByIdAsync(Guid id);
    Task<ArtworkImage?> GetByArtworkRefIdAsync(int artworkRefId);
    Task AddAsync(ArtworkImage image);
    void Delete(ArtworkImage image);
    Task SaveChangesAsync();
}