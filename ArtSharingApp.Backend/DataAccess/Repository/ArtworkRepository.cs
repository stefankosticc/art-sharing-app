using System.Linq.Expressions;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;

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

    public new IEnumerable<Artwork> GetAll(params Expression<Func<Artwork, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<Artwork, object>>>()).ToArray();
        return base.GetAll(combinedIncludes);
    }
    
    public new Artwork GetById(object id, params Expression<Func<Artwork, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<Artwork, object>>>()).ToArray();

        return base.GetById(id, combinedIncludes);
    }
}