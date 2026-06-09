using ArtSharingApp.ImageService.DataAccess.Repository;
using ArtSharingApp.ImageService.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.ImageService.DataAccess.Repository;

public class ArtworkImageRepository : IArtworkImageRepository
{
    private readonly ImageDbContext _context;
    private readonly DbSet<ArtworkImage> _dbSet;

    public ArtworkImageRepository(ImageDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<ArtworkImage>();
    }

    public async Task<ArtworkImage?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task<ArtworkImage?> GetByArtworkRefIdAsync(int artworkRefId)
        => await _dbSet.FirstOrDefaultAsync(a => a.ArtworkRefId == artworkRefId);

    public async Task AddAsync(ArtworkImage image)
        => await _dbSet.AddAsync(image);

    public void Delete(ArtworkImage image)
        => _dbSet.Remove(image);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}