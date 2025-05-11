using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

public class GalleryService : IGalleryService
{
    private readonly IGenericRepository<Gallery> _galleryRepository;
    private readonly IMapper _mapper;
    
    public GalleryService(IGenericRepository<Gallery> galleryRepository, IMapper mapper)
    {
        _galleryRepository = galleryRepository;
        _mapper = mapper;
    }

    public IEnumerable<GalleryResponseDTO> GetAll()
    {
        var galleries = _galleryRepository.GetAll();
        return _mapper.Map<IEnumerable<GalleryResponseDTO>>(galleries);
    }

    public GalleryResponseDTO? GetById(int id)
    {
        var gallery = _galleryRepository.GetById(id);
        if (gallery == null)
            return null;
        return _mapper.Map<GalleryResponseDTO>(gallery);
    }

    public void Add(GalleryRequestDTO galleryDto)
    {
        var gallery = _mapper.Map<Gallery>(galleryDto);
        _galleryRepository.Add(gallery);
        _galleryRepository.Save();
    }

    public void Update(int id, GalleryRequestDTO galleryDto)
    {
        if (_galleryRepository.GetById(id) == null) return;
        var gallery = _mapper.Map<Gallery>(galleryDto);
        gallery.Id = id;
        _galleryRepository.Update(gallery);
        _galleryRepository.Save();
    }

    public void Delete(int id)
    {
        var gallery = _galleryRepository.GetById(id);
        if (gallery == null) return;
        _galleryRepository.Delete(gallery);
        _galleryRepository.Save();
    }
}
