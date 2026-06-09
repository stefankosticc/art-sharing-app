using ArtSharingApp.ImageService.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.ImageService.DataAccess;

public class ImageDbContext : DbContext
{
    public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
    {
    }

    public DbSet<ArtworkImage> ArtworkImages { get; set; }
    public DbSet<UserProfilePhoto> UserProfilePhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArtworkImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageData).IsRequired();
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.ArtworkRefId).IsUnique();
        });

        modelBuilder.Entity<UserProfilePhoto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageData).IsRequired();
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.UserRefId).IsUnique();
        });
    }
}