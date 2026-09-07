using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace ArtSharingApp.Backend.Utils;

public static class ImageUrlSigner
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(10);
    private static string _secret = string.Empty;

    public static void Configure(string secret)
    {
        _secret = secret;
    }

    public static string Sign(string imageId)
    {
        var exp = DateTimeOffset.UtcNow.Add(Ttl).ToUnixTimeSeconds();
        var signature = Compute(imageId, exp);
        return $"exp={exp}&sig={signature}";
    }

    private static string Compute(string imageId, long exp)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_secret);
        var messageBytes = Encoding.UTF8.GetBytes($"{imageId}:{exp}");
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(messageBytes);
        return WebEncoders.Base64UrlEncode(hash);
    }
}
