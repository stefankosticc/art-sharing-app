using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface;

public interface IFavoritesService
{
    Task<bool> LikeArtwork(int userId, int artworkId);
    Task<bool> DislikeArtwork(int userId, int artworkId);
    Task<IEnumerable<FavoritesDTO>?> GetLikedArtworks(int userId);
    Task<IEnumerable<TopArtistResponseDTO>?> GetTop10ArtistsByLikesAsync();
    Task<IEnumerable<DiscoverArtworkDTO>?> GetTrendingArtworksAsync(int count);
}