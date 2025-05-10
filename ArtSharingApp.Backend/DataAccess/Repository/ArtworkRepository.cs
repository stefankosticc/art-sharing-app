using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class ArtworkRepository : GenericRepository<Artwork>,IArtworkRepository
{
    public ArtworkRepository(ApplicationDbContext context) : base(context)
    {
    }
}