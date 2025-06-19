using System.Linq.Expressions;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class ArtworkRepository : GenericRepository<Artwork>,IArtworkRepository
{
    protected Expression<Func<Artwork, object>>[] DefaultIncludes => new Expression<Func<Artwork, object>>[]
    {
        a => a.CreatedByArtist,
        a => a.PostedByUser, 
        a => a.City, 
        a => a.Gallery
    };

    public ArtworkRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Artwork>> GetAllAsync(params Expression<Func<Artwork, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<Artwork, object>>>()).ToArray();
        return await base.GetAllAsync(combinedIncludes);
    }
    
    public async Task<Artwork> GetByIdAsync(object id, params Expression<Func<Artwork, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<Artwork, object>>>()).ToArray();
        return await base.GetByIdAsync(id, combinedIncludes);
    }

    public async Task<IEnumerable<Artwork>?> SearchByTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            return null;
        var artworks = await _context.Artworks.Where(a => a.Title.ToLower().Contains(title.ToLower())).ToListAsync();
        return artworks;
    }

    public void UpdateIsPrivate(Artwork artwork)
    {
        _context.Attach(artwork);
        _context.Entry(artwork).Property(a => a.IsPrivate).IsModified = true;
    }

    public void UpdateSaleProperties(Artwork artwork)
    {
        _context.Attach(artwork);
        _context.Entry(artwork).Property(a => a.IsOnSale).IsModified = true;
        _context.Entry(artwork).Property(a => a.Price).IsModified = true;
        _context.Entry(artwork).Property(a => a.Currency).IsModified = true;
    }

    public void UpdateOwner(Artwork artwork)
    {
        _context.Attach(artwork);
        _context.Entry(artwork).Property(a => a.PostedByUserId).IsModified = true;
    }

    public async Task<IEnumerable<Artwork>?> GetMyArtworksAsync(int postedByUserId)
    {
        return await _dbSet
            .Where(a => a.PostedByUserId == postedByUserId)
            .Include(a => a.PostedByUser)
            .ToListAsync();
    }
}