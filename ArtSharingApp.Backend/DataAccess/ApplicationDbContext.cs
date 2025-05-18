using Microsoft.EntityFrameworkCore;
using ArtSharingApp.Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
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
    public DbSet<Followers> Followers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        
        modelBuilder.Entity<Gallery>()
            .HasOne(g => g.City)
            .WithMany(c => c.Galleries)
            .HasForeignKey(g => g.CityId)
            .IsRequired();

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.City)
            .WithMany(c => c.Artworks)
            .HasForeignKey(a => a.CityId)
            .IsRequired(false);

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.Gallery)
            .WithMany(g => g.Artworks)
            .HasForeignKey(a => a.GalleryId)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.CreatedByArtist)
            .WithMany(u => u.CreatedArtworks)
            .HasForeignKey(a => a.CreatedByArtistId)
            .IsRequired();

        modelBuilder.Entity<Artwork>()
            .HasOne(a => a.PostedByUser)
            .WithMany(u => u.PostedArtworks)
            .HasForeignKey(a => a.PostedByUserId)
            .IsRequired();

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
        
        modelBuilder.Entity<Followers>()
            .HasKey(f => new { f.UserId, f.FollowerId });
        
        modelBuilder.Entity<Followers>()
            .HasOne(f => f.User)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.UserId)
            .IsRequired();
        
        modelBuilder.Entity<Followers>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId)
            .IsRequired();
    }
}
