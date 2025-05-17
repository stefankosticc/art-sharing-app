using ArtSharingApp.Backend.DataAccess.Repository;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.DataAccess;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Exceptions.ErrorHandler;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Profile;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register ApplicationDbContext with PostgreSQL provider using connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ArtSharingAppContext")));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });

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
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandler>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
