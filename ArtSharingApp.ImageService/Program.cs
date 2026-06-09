using ArtSharingApp.ImageService.DataAccess;
using ArtSharingApp.ImageService.DataAccess.Repository;
using ArtSharingApp.ImageService.Exceptions;
using ArtSharingApp.ImageService.Middleware;
using ArtSharingApp.ImageService.Profiles;
using ArtSharingApp.ImageService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ImageDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ImageContext")));

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ArtworkImageProfile).Assembly));

builder.Services.AddScoped<IArtworkImageRepository, ArtworkImageRepository>();
builder.Services.AddScoped<IUserProfilePhotoRepository, UserProfilePhotoRepository>();

builder.Services.AddScoped<IArtworkImageService, ArtworkImageService>();
builder.Services.AddScoped<IUserProfilePhotoService, UserProfilePhotoService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandler>();

app.UseMiddleware<ApiKeyMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Image Service API"); });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();