using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.DataAccess;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Profile;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register ApplicationDbContext with PostgreSQL provider using connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArtSharingAppContext")));

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArtworkRepository, ArtworkRepository>();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IArtworkService, ArtworkService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<IFavoritesService, FavoritesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Open API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
