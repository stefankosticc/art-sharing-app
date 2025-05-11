namespace ArtSharingApp.Backend.DTO;

// DTO for returning user data to clients
public class UserResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string? Biography { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
}

// DTO for creating/updating users (request)
public class UserRequestDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Biography { get; set; }
    public int RoleId { get; set; }
}
