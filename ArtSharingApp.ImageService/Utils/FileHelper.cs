namespace ArtSharingApp.ImageService.Utils;

public static class FileHelper
{
    /// <summary>
    /// Reads the contents of an uploaded file into a byte array.
    /// </summary>
    /// <param name="file">The uploaded file to read.</param>
    /// <returns>A byte array containing the file's data.</returns>
    public static async Task<byte[]> ReadFileBytesAsync(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }
}