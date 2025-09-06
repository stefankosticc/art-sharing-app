using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using ArtSharingApp.Backend.Exceptions;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing users.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IFollowersRepository _followersRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    /// <param name="followersRepository">Repository for followers data access.</param>
    public UserService(IUserRepository userRepository, IMapper mapper, IFollowersRepository followersRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _followersRepository = followersRepository;
    }

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="userDto">The user data.</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    public async Task AddUserAsync(UserRequestDTO userDto)
    {
        if (userDto == null)
            throw new BadRequestException("User parameters not provided correctly.");
        var user = _mapper.Map<User>(userDto);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();
    }

    /// <summary>
    /// Updates an existing user's profile, including profile photo.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="userDto">The updated user profile data.</param>
    /// <param name="profilePhoto">The new profile photo file (optional).</param>
    /// <exception cref="BadRequestException">Thrown if parameters are invalid.</exception>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
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

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The <see cref="UserResponseDTO"/> representing the user with the specified ID.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
    public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} not found.");
        return _mapper.Map<UserResponseDTO>(user);
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <returns>The <see cref="UserByUserNameResponseDTO"/> representing the user with the specified username.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
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

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A collection of <see cref="UserResponseDTO"/> representing all users.</returns>
    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} not found.");
        await _userRepository.DeleteAsync(id);
        await _userRepository.SaveAsync();
    }

    /// <summary>
    /// Updates the biography of a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="biography">The new biography text.</param>
    /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="BadRequestException">Thrown if the biography update fails.</exception>
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

    /// <summary>
    /// Searches for users by name or username.
    /// </summary>
    /// <param name="searchString">The search string for name or username.</param>
    /// <returns>A collection of <see cref="UserSearchResponseDTO"/> matching the search criteria.</returns>
    /// <exception cref="BadRequestException">Thrown if the search string is invalid.</exception>
    public async Task<IEnumerable<UserSearchResponseDTO?>> GetUsersByNameAndUserName(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            throw new BadRequestException("Username parameter is required.");
        var users = await _userRepository.GetUsersByNameAndUserName(searchString);
        return _mapper.Map<IEnumerable<UserSearchResponseDTO>>(users);
    }

    /// <summary>
    /// Retrieves the profile photo and content type for a user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A tuple containing the profile photo bytes and content type.</returns>
    /// <exception cref="NotFoundException">Thrown if the profile photo is not found.</exception>
    public async Task<(byte[] ProfilePhoto, string ContentType)> GetProfilePhotoAsync(int id)
    {
        var result = await _userRepository.GetProfilePhotoAsync(id);
        if (result.ProfilePhoto == null || result.ProfilePhoto.Length == 0)
            throw new NotFoundException("Profile photo not found.");

        return (result.ProfilePhoto, string.IsNullOrWhiteSpace(result.ContentType) ? "image/jpeg" : result.ContentType);
    }
}