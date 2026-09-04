using ArtSharingApp.ImageService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtSharingApp.ImageService.Controllers;

[ApiController]
[Route("images/users/profile-photo")]
public class UserProfilePhotoController : ControllerBase
{
    private readonly IUserProfilePhotoService _userProfilePhotoService;

    public UserProfilePhotoController(IUserProfilePhotoService userProfilePhotoService)
    {
        _userProfilePhotoService = userProfilePhotoService;
    }

    [HttpPost]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> Upload([FromQuery] int userRefId, IFormFile image)
    {
        var result = await _userProfilePhotoService.UploadAsync(userRefId, image);
        return Ok(result);
    }

    [HttpGet("{photoId:guid}")]
    public async Task<IActionResult> Get(Guid photoId)
    {
        var (data, contentType) = await _userProfilePhotoService.GetByIdAsync(photoId);
        return File(data, contentType);
    }

    [HttpPut("{photoId:guid}")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> Replace(Guid photoId, IFormFile image)
    {
        var result = await _userProfilePhotoService.ReplaceAsync(photoId, image);
        return Ok(result);
    }

    [HttpDelete("{photoId:guid}")]
    public async Task<IActionResult> Delete(Guid photoId)
    {
        await _userProfilePhotoService.DeleteAsync(photoId);
        return Ok(new { message = "Profile photo deleted successfully." });
    }
}
