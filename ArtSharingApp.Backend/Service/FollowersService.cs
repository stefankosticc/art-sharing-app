using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing user followers and following relationships.
/// </summary>
public class FollowersService : IFollowersService
{
    private readonly IFollowersRepository _followersRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FollowersService"/> class.
    /// </summary>
    /// <param name="followersRepository">Repository for followers data access.</param>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    /// <param name="notificationService">Service for sending notifications.</param>
    public FollowersService(
        IFollowersRepository followersRepository,
        IUserRepository userRepository,
        IMapper mapper,
        INotificationService notificationService)
    {
        _followersRepository = followersRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Follows a user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user who wants to follow another user.</param>
    /// <param name="userId">The ID of the user to be followed.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if already following or trying to follow self.</exception>
    /// <exception cref="NotFoundException">Thrown if the user to be followed is not found.</exception>
    public async Task<bool> FollowUserAsync(int loggedInUserId, int userId)
    {
        var isFollowing = await _followersRepository.IsFollowing(loggedInUserId, userId);
        if (isFollowing)
            throw new BadRequestException("You are already following this user.");

        if (loggedInUserId == userId)
            throw new BadRequestException("You cannot follow yourself.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        await _followersRepository.AddAsync(new Followers(loggedInUserId, userId));
        await _followersRepository.SaveAsync();

        // Send notification to the user being followed
        var loggedInUser = await _userRepository.GetByIdAsync(loggedInUserId);
        var notification = new NotificationRequestDTO
        {
            RecipientId = userId,
            Text = $"@{loggedInUser.UserName} started following you."
        };
        await _notificationService.CreateNotificationAsync(notification);

        return true;
    }

    /// <summary>
    /// Unfollows a user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user who wants to unfollow another user.</param>
    /// <param name="userId">The ID of the user to be unfollowed.</param>
    /// <returns>True if the operation succeeds.</returns>
    /// <exception cref="BadRequestException">Thrown if not following or trying to unfollow self.</exception>
    /// <exception cref="NotFoundException">Thrown if the user to be unfollowed is not found.</exception>
    public async Task<bool> UnfollowUserAsync(int loggedInUserId, int userId)
    {
        if (loggedInUserId == userId)
            throw new BadRequestException("You cannot unfollow yourself.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        var isFollowing = await _followersRepository.IsFollowing(loggedInUserId, userId);
        if (!isFollowing)
            throw new BadRequestException("You are not following this user.");

        await _followersRepository.DeleteAsync(loggedInUserId, userId);
        await _followersRepository.SaveAsync();
        return true;
    }

    /// <summary>
    /// Retrieves the followers of a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose followers to retrieve.</param>
    /// <param name="skip">The number of followers to skip for pagination.</param>
    /// <param name="take">The number of followers to take for pagination.</param>
    /// <returns>A collection of <see cref="FollowersDTO"/> representing the user's followers.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found or has no followers.</exception>
    public async Task<IEnumerable<FollowersDTO>?> GetFollowersAsync(int userId, int skip, int take)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        var followers = await _followersRepository.GetFollowersAsync(userId, skip, take);
        if (followers == null || !followers.Any())
            throw new NotFoundException("No followers found.");

        var followersDto = _mapper.Map<IEnumerable<FollowersDTO>>(followers);
        return followersDto;
    }

    /// <summary>
    /// Retrieves the users that a user is following.
    /// </summary>
    /// <param name="userId">The ID of the user whose following list to retrieve.</param>
    /// <param name="skip">The number of users to skip for pagination.</param>
    /// <param name="take">The number of users to take for pagination.</param>
    /// <returns>A collection of <see cref="FollowingDTO"/> representing the users being followed.</returns>
    /// <exception cref="NotFoundException">Thrown if the user is not found or not following anyone.</exception>
    public async Task<IEnumerable<FollowingDTO>?> GetFollowingAsync(int userId, int skip, int take)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        var following = await _followersRepository.GetFollowingAsync(userId, skip, take);
        if (following == null || !following.Any())
            throw new NotFoundException("You are not following anyone.");

        var followingDto = _mapper.Map<IEnumerable<FollowingDTO>>(following);
        return followingDto;
    }

    /// <summary>
    /// Retrieves the count of followers for a user.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user whose followers count to retrieve.</param>
    /// <returns>The number of followers.</returns>
    public async Task<int> GetFollowersCountAsync(int loggedInUserId)
    {
        var followersCount = await _followersRepository.GetFollowersCountAsync(loggedInUserId);
        return followersCount;
    }

    /// <summary>
    /// Retrieves the count of users that a user is following.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the user whose following count to retrieve.</param>
    /// <returns>The number of users being followed.</returns>
    public async Task<int> GetFollowingCountAsync(int loggedInUserId)
    {
        var followingCount = await _followersRepository.GetFollowingCountAsync(loggedInUserId);
        return followingCount;
    }

    /// <summary>
    /// Retrieves artworks posted by users that the logged-in user is following.
    /// </summary>
    /// <param name="loggedInUserId">The ID of the logged-in user.</param>
    /// <param name="skip">The number of artworks to skip for pagination.</param>
    /// <param name="take">The number of artworks to take for pagination.</param>
    /// <returns>A collection of <see cref="FollowedUserArtworkDTO"/> representing artworks from followed users.</returns>
    public async Task<IEnumerable<FollowedUserArtworkDTO>?> GetFollowedUsersArtworksAsync(int loggedInUserId, int skip, int take)
    {
        var artworks = await _followersRepository.GetFollowedUsersArtworksAsync(loggedInUserId, skip, take);
        return _mapper.Map<IEnumerable<FollowedUserArtworkDTO>>(artworks);
    }
}