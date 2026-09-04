using ArtSharingApp.ImageService.DataAccess.Repository;
using ArtSharingApp.ImageService.DTO;
using ArtSharingApp.ImageService.Exceptions;
using ArtSharingApp.ImageService.Models;
using ArtSharingApp.ImageService.Utils;
using AutoMapper;

namespace ArtSharingApp.ImageService.Services;

public class UserProfilePhotoService : IUserProfilePhotoService
{
    private readonly IUserProfilePhotoRepository _userProfilePhotoRepository;
    private readonly IMapper _mapper;

    public UserProfilePhotoService(IUserProfilePhotoRepository userProfilePhotoRepository, IMapper mapper)
    {
        _userProfilePhotoRepository = userProfilePhotoRepository;
        _mapper = mapper;
    }

    public async Task<UserProfilePhotoDTO> UploadAsync(int userRefId, IFormFile file)
    {
        var imageBytes = await FileHelper.ReadFileBytesAsync(file);

        var photo = UserProfilePhoto.Create(userRefId, imageBytes, file.ContentType);
        await _userProfilePhotoRepository.AddAsync(photo);
        await _userProfilePhotoRepository.SaveChangesAsync();

        return _mapper.Map<UserProfilePhotoDTO>(photo);
    }

    public async Task<(byte[] Data, string ContentType)> GetByIdAsync(Guid photoId)
    {
        var photo = await _userProfilePhotoRepository.GetByIdAsync(photoId);
        if (photo?.ImageData == null || photo.ImageData.Length == 0)
            throw new NotFoundException("Profile photo not found.");

        return (photo.ImageData, string.IsNullOrWhiteSpace(photo.ContentType) ? "image/jpeg" : photo.ContentType);
    }

    public async Task<UserProfilePhotoDTO> ReplaceAsync(Guid photoId, IFormFile file)
    {
        var photo = await _userProfilePhotoRepository.GetByIdAsync(photoId)
                    ?? throw new NotFoundException($"Profile photo {photoId} not found.");

        var imageBytes = await FileHelper.ReadFileBytesAsync(file);
        photo.Update(imageBytes, file.ContentType);
        await _userProfilePhotoRepository.SaveChangesAsync();

        return _mapper.Map<UserProfilePhotoDTO>(photo);
    }

    public async Task DeleteAsync(Guid photoId)
    {
        var photo = await _userProfilePhotoRepository.GetByIdAsync(photoId)
                    ?? throw new NotFoundException($"Profile photo {photoId} not found.");

        _userProfilePhotoRepository.Delete(photo);
        await _userProfilePhotoRepository.SaveChangesAsync();
    }
}