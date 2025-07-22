using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IFavoritesRepository : IGenericRepository<Favorites>
{
    Task DeleteAsync(int userId, int artworkId);
    Task<IEnumerable<Favorites>> GetLikedArtworks(int userId);
    Task<IEnumerable<User>> GetTopArtistsByLikesAsync(int count);
}