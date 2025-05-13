namespace ArtSharingApp.Backend.Models;

public class Favorites
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int ArtworkId { get; set; }
    public Artwork Artwork { get; set; }
    
    public Favorites(int userId, int artworkId)
    {
        UserId = userId;
        ArtworkId = artworkId;
    }
}
