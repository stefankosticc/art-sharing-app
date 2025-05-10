using Microsoft.EntityFrameworkCore;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<Artwork> Artworks { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Favorites> Favorites { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gallery>()
            .HasOne(g => g.City)
            .WithMany(c => c.Galleries)
            .HasForeignKey(g => g.CityId)
            .IsRequired(); // 1,1 to 0,Many

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.City)
            .WithMany(c => c.Artworks)
            .HasForeignKey(a => a.CityId)
            .IsRequired(false); // 0,1 to 0,Many

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.Gallery)
            .WithMany(g => g.Artworks)
            .HasForeignKey(a => a.GalleryId)
            .IsRequired(false); // 0,1 to 0,Many

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .IsRequired(); // 1,1 to 0,Many

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.CreatedByArtist)
            .WithMany(u => u.CreatedArtworks)
            .HasForeignKey(a => a.CreatedByArtistId)
            .IsRequired(); // 1,1 to 0,Many

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.PostedByUser)
            .WithMany(u => u.PostedArtworks)
            .HasForeignKey(a => a.PostedByUserId)
            .IsRequired(); // 1,1 to 0,Many

        modelBuilder.Entity<Favorites>()
            .HasKey(f => new { f.UserId, f.ArtworkId });

        modelBuilder.Entity<Favorites>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .IsRequired();

        modelBuilder.Entity<Favorites>()
            .HasOne(f => f.Artwork)
            .WithMany(a => a.Favorites)
            .HasForeignKey(f => f.ArtworkId)
            .IsRequired();
    }
}
