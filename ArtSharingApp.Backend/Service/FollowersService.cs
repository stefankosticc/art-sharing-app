using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace ArtSharingApp.Backend.Service;

public class FollowersService : IFollowersService
{
    private readonly IFollowersRepository _followersRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

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

    // Get followers of the logged-in user
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

    // Get users that the logged-in user is following
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

    public async Task<int> GetFollowersCountAsync(int loggedInUserId)
    {
        var followersCount = await _followersRepository.GetFollowersCountAsync(loggedInUserId);
        return followersCount;
    }

    public async Task<int> GetFollowingCountAsync(int loggedInUserId)
    {
        var followingCount = await _followersRepository.GetFollowingCountAsync(loggedInUserId);
        return followingCount;
    }

    public async Task<IEnumerable<FollowedUserArtworkDTO>?> GetFollowedUsersArtworksAsync(int loggedInUserId, int skip,
        int take)
    {
        var artworks = await _followersRepository.GetFollowedUsersArtworksAsync(loggedInUserId, skip, take);
        return _mapper.Map<IEnumerable<FollowedUserArtworkDTO>>(artworks);
    }
}