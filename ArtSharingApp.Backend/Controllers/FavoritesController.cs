using ArtSharingApp.Backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class FavoritesController : Controller
{
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    private int GetLoggedInUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User not authenticated.");
        return userId;
    }

    [HttpPost("artwork/{artworkId}/like")]
    public async Task<IActionResult> LikeArtwork(int artworkId)
    {
        var userId = GetLoggedInUserId();
        var liked = await _favoritesService.LikeArtwork(userId, artworkId);
        if (liked)
        {
            return Ok(new { message = "Artwork liked successfully." });
        }
        return BadRequest(new { message = "Failed to like artwork." });
    }

    [HttpDelete("artwork/{artworkId}/dislike")]
    public async Task<IActionResult> DislikeArtwork(int artworkId)
    {
        var userId = GetLoggedInUserId();
        var disliked = await _favoritesService.DislikeArtwork(userId, artworkId);
        if (disliked)
        {
            return Ok(new { message = "Artwork disliked successfully." });
        }
        return BadRequest(new { message = "Failed to dislike artwork." });
    }

    [HttpGet("user/liked-artworks")]
    public async Task<IActionResult> GetLikedArtworks()
    {
        var userId = GetLoggedInUserId();
        var likedArtworks = await _favoritesService.GetLikedArtworks(userId);
        if (likedArtworks != null)
        {
            return Ok(likedArtworks);
        }
        throw new NotFoundException("No liked artworks found.");
    }
}
