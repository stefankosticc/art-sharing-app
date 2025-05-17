using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace ArtSharingApp.Backend.Models;

public class Role : IdentityRole<int>
{
    // public int Id { get; set; }
    // public string Name { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}
