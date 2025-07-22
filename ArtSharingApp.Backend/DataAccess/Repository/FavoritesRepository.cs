using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class FavoritesRepository : GenericRepository<Favorites>, IFavoritesRepository
{
    public FavoritesRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task DeleteAsync(int userId, int artworkId)
    {
        var favorite = await _dbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.ArtworkId == artworkId);
        if (favorite != null)
        {
            _dbSet.Remove(favorite);
        }
    }

    public async Task<IEnumerable<Favorites>> GetLikedArtworks(int userId)
    {
        return await _dbSet
            .Where(f => f.UserId == userId)
            .Include(f => f.Artwork)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetTopArtistsByLikesAsync(int count)
    {
        var topArtistLikes = await _dbSet
            .Where(f => !f.Artwork.IsPrivate)
            .GroupBy(f => f.Artwork.CreatedByArtistId)
            .Select(g => new { ArtistId = g.Key, TotalLikes = g.Count() })
            .OrderByDescending(x => x.TotalLikes)
            .Take(count)
            .ToListAsync();

        var topArtistIds = topArtistLikes.Select(x => x.ArtistId).ToList();

        var artists = await _context.Users.Where(u => topArtistIds.Contains(u.Id)).ToListAsync();

        // Preserve ranking order
        var artistDict = artists.ToDictionary(a => a.Id);
        return topArtistIds.Select(id => artistDict[id]).ToList();
    }
}