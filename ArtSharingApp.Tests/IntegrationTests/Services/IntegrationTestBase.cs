using ArtSharingApp.Backend.DataAccess;
using ArtSharingApp.Backend.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArtSharingApp.Tests.IntegrationTests.Services;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly IServiceProvider ServiceProvider;

    protected IntegrationTestBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(configuration.GetConnectionString("ArtSharingAppContext"))
            .Options;

        DbContext = new ApplicationDbContext(options);

        DbContext.Database.EnsureDeleted();
        DbContext.Database.Migrate();

        var services = new ServiceCollection();
        services.AddScoped<ApplicationDbContext>(_ => DbContext);
        services.AddAutoMapper(typeof(UserProfile).Assembly);
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}