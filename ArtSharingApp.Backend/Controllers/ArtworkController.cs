using ArtSharingApp.Backend.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class ArtworkController : AuthenticatedUserBaseController
{
    private readonly IArtworkService _artworkService;

    public ArtworkController(IArtworkService artworkService)
    {
        _artworkService = artworkService;
    }

    [HttpGet("artworks")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _artworkService.GetAllAsync());
    }

    [HttpGet("artwork/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var artwork = await _artworkService.GetByIdAsync(id);
        return Ok(artwork);
    }

    [Authorize(Roles = "Admin, Artist")]
    [HttpPost("artwork")]
    public async Task<IActionResult> Add([FromBody] ArtworkRequestDTO artworkDto)
    {
        await _artworkService.AddAsync(artworkDto);
        return Ok(new {message = "Artwork added successfully."});
    }

    [Authorize(Roles = "Admin, Artist")]
    [HttpPut("artwork/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ArtworkRequestDTO artworkDto)
    {
        await _artworkService.UpdateAsync(id, artworkDto);
        return Ok(new {message = "Artwork updated successfully."});
    }

    [Authorize(Roles = "Admin, Artist")]
    [HttpDelete("artwork/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _artworkService.DeleteAsync(id);
        return Ok(new {message = "Artwork deleted successfully."});
    }
    
    [HttpGet("artworks/search")]
    public async Task<IActionResult> Search([FromQuery] string title)
    {
        var artworks = await _artworkService.SearchByTitle(title);
        return Ok(artworks);
    }
    
    [Authorize(Roles = "Admin, Artist")]
    [HttpPut("artwork/{id}/change-visibility")]
    public async Task<IActionResult> ChangeVisibility(int id, [FromBody] ChangeArtworkVisibilityDTO request)
    {
        var isPrivate = request.IsPrivate;
        await _artworkService.ChangeVisibilityAsync(id, isPrivate);
        return Ok(new {message = $"Artwork visibility changed successfully to {(isPrivate ? "private" : "public")}."});
    }
    
    [HttpPut("artwork/{id}/put-on-sale")]
    public async Task<IActionResult> PutOnSale(int id, [FromBody] PutArtworkOnSaleDTO request)
    {
        var loggedInUserId = GetLoggedInUserId();
        await _artworkService.PutOnSaleAsync(id, loggedInUserId, request);
        return Ok(new {message = "Artwork put on sale successfully."});
    }
    
    [HttpPut("artwork/{id}/remove-from-sale")]
    public async Task<IActionResult> RemoveFromSale(int id)
    {
        var loggedInUserId = GetLoggedInUserId();
        await _artworkService.RemoveFromSaleAsync(id, loggedInUserId);
        return Ok(new {message = "Artwork removed from sale successfully."});
    }
}
