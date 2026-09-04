using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IGalleryRepository : IGenericRepository<Gallery>
{
    Task<IEnumerable<Gallery>> GetGalleriesByName(string name);
    Task<IEnumerable<Gallery>> GetGalleriesInBoundingBox(double south, double west, double north, double east);
}