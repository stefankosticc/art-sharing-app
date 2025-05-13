using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class FavoritesController : Controller
{
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }
    
    [HttpPost("user/{userId}/artwork/{artworkId}/like")]
    public async Task<IActionResult> LikeArtwork(int userId, int artworkId)
    {
        var liked = await _favoritesService.LikeArtwork(userId, artworkId);
        if (liked)
        {
            return Ok(new { message = "Artwork liked successfully." });
        }
        return BadRequest(new { message = "Failed to like artwork." });
    }
    
    [HttpDelete("user/{userId}/artwork/{artworkId}/dislike")]
    public async Task<IActionResult> DislikeArtwork(int userId, int artworkId)
    {
        var disliked = await _favoritesService.DislikeArtwork(userId, artworkId);
        if (disliked)
        {
            return Ok(new { message = "Artwork disliked successfully." });
        }
        return BadRequest(new { message = "Failed to dislike artwork." });
    }
    
    [HttpGet("user/{userId}/liked-artworks")]
    public async Task<IActionResult> GetLikedArtworks(int userId)
    {
        var likedArtworks = await _favoritesService.GetLikedArtworks(userId);
        if (likedArtworks != null)
        {
            return Ok(likedArtworks);
        }
        return NotFound(new { message = "No liked artworks found." });
    }
}
