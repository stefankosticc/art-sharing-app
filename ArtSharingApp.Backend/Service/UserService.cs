using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IFollowersRepository _followersRepository;

    public UserService(IUserRepository userRepository, IMapper mapper, IFollowersRepository followersRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _followersRepository = followersRepository;
    }

    public async Task AddUserAsync(UserRequestDTO userDto)
    {
        if (userDto == null)
            throw new BadRequestException("User parameters not provided correctly.");
        var user = _mapper.Map<User>(userDto);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();
    }

    public async Task UpdateAsync(int userId, UpdateUserProfileRequestDTO userDto, IFormFile? profilePhoto)
    {
        if (userDto == null)
            throw new BadRequestException("User parameters not provided correctly.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException($"User with id {userId} not found.");

        _mapper.Map(userDto, user);

        if (profilePhoto != null && profilePhoto.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                await profilePhoto.CopyToAsync(ms);
                user.UpdateProfilePhoto(ms.ToArray(), profilePhoto.ContentType);
            }
        }
        else if (userDto.RemovePhoto)
        {
            user.RemoveProfilePhoto();
        }

        _userRepository.UpdateUserProfile(user);
        await _userRepository.SaveAsync();
    }

    public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} not found.");
        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<UserByUserNameResponseDTO?> GetUserByUserNameAsync(string username, int loggedInUserId)
    {
        var user = await _userRepository.GetUserByUserNameAsync(username);
        if (user == null)
            throw new NotFoundException($"User with username @{username} not found.");
        var response = _mapper.Map<UserByUserNameResponseDTO>(user);

        if (loggedInUserId != user.Id)
            response.IsFollowedByLoggedInUser = (await _followersRepository.GetAllAsync())
                .Any(f => f.UserId == loggedInUserId && f.FollowerId == user.Id);
        else
            response.IsFollowedByLoggedInUser = null;

        response.FollowersCount = await _followersRepository.GetFollowersCountAsync(user.Id);
        response.FollowingCount = await _followersRepository.GetFollowingCountAsync(user.Id);

        return response;
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} not found.");
        await _userRepository.DeleteAsync(id);
        await _userRepository.SaveAsync();
    }

    public async Task UpdateUserBiographyAsync(int userId, string biography)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException($"User with id {userId} not found.");

        try
        {
            user.UpdateBiography(biography);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }

        _userRepository.UpdateBiography(user);
        await _userRepository.SaveAsync();
    }

    public async Task<IEnumerable<UserSearchResponseDTO?>> GetUsersByNameAndUserName(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            throw new BadRequestException("Username parameter is required.");
        var users = await _userRepository.GetUsersByNameAndUserName(searchString);
        return _mapper.Map<IEnumerable<UserSearchResponseDTO>>(users);
    }

    public async Task<(byte[] ProfilePhoto, string ContentType)> GetProfilePhotoAsync(int id)
    {
        var result = await _userRepository.GetProfilePhotoAsync(id);
        if (result.ProfilePhoto == null || result.ProfilePhoto.Length == 0)
            throw new NotFoundException("Profile photo not found.");

        return (result.ProfilePhoto, string.IsNullOrWhiteSpace(result.ContentType) ? "image/jpeg" : result.ContentType);
    }
}