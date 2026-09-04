namespace ArtSharingApp.ImageService.Middleware;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeader = "X-Api-Key";
    private readonly RequestDelegate _next;
    private readonly string _configuredApiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuredApiKey = configuration["ApiKey"]
            ?? throw new InvalidOperationException("ApiKey is not configured.");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsWriteRequest(context))
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var providedKey)
                || providedKey != _configuredApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid or missing API key." });
                return;
            }
        }

        await _next(context);
    }

    private static bool IsWriteRequest(HttpContext context)
    {
        var method = context.Request.Method;
        return HttpMethods.IsPost(method)
            || HttpMethods.IsPut(method)
            || HttpMethods.IsDelete(method);
    }
}
