using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace ArtSharingApp.ImageService.Utils;

public static class ImageUrlVerifier
{
    private static string _secret = string.Empty;

    public static void Configure(string secret)
    {
        _secret = secret;
    }

    public static bool IsValid(Guid imageId, long exp, string? sig)
    {
        if (string.IsNullOrEmpty(sig))
            return false;

        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > exp)
            return false;

        byte[] provided;
        try
        {
            provided = WebEncoders.Base64UrlDecode(sig);
        }
        catch (FormatException)
        {
            return false;
        }

        var keyBytes = Encoding.UTF8.GetBytes(_secret);
        var messageBytes = Encoding.UTF8.GetBytes($"{imageId}:{exp}");
        using var hmac = new HMACSHA256(keyBytes);
        var expected = hmac.ComputeHash(messageBytes);

        return CryptographicOperations.FixedTimeEquals(expected, provided);
    }
}
