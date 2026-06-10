using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IArtworkRepository : IGenericRepository<Artwork>
{
    Task<IEnumerable<Artwork>?> SearchByTitle(string title);
    void UpdateIsPrivate(Artwork artwork);
    void UpdateSaleProperties(Artwork artwork);
    void UpdateOwner(Artwork artwork);
Task<IEnumerable<Artwork>?> GetDiscoverArtworksAsync(int loggedInUserId, int skip, int take);
    Task<IEnumerable<Artwork>?> GetPrivateArtworksByUserAsync(int postedByUserId);
    Task<IEnumerable<Artwork>?> GetPublicArtworksByUserAsync(int postedByUserId);
}