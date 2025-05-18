using ArtSharingApp.Backend.Controllers.Common;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class FollowersController : AuthenticatedUserBaseController
{
    private readonly IFollowersService _followersService;
    
    public FollowersController(IFollowersService followersService)
    {
        _followersService = followersService;
    }
    
    [HttpPost("follow/{userId}")]
    public async Task<IActionResult> FollowUser(int userId)
    {
        var loggedInUserId = GetLoggedInUserId();
        var followed = await _followersService.FollowUserAsync(loggedInUserId, userId);
        if (followed)
            return Ok(new { message = "User followed successfully." });
        return BadRequest(new { message = "Failed to follow user." });
    }
    
    [HttpDelete("unfollow/{userId}")]
    public async Task<IActionResult> UnfollowUser(int userId)
    {
        var loggedInUserId = GetLoggedInUserId();
        var unfollowed = await _followersService.UnfollowUserAsync(loggedInUserId, userId);
        if (unfollowed)
            return Ok(new { message = "User unfollowed successfully." });
        return BadRequest(new { message = "Failed to unfollow user." });
    }
    
    [HttpGet("followers")]
    public async Task<IActionResult> GetFollowers()
    {
        var loggedInUserId = GetLoggedInUserId();
        var followers = await _followersService.GetFollowersAsync(loggedInUserId);
        return Ok(followers);
    }
    
    [HttpGet("following")]
    public async Task<IActionResult> GetFollowing()
    {
        var loggedInUserId = GetLoggedInUserId();
        var following = await _followersService.GetFollowingAsync(loggedInUserId);
        return Ok(following);
    }
}