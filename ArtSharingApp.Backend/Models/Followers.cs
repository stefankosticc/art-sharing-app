namespace ArtSharingApp.Backend.Models;

public class Followers
{
    // UserId -- follows --> FollowerId
    public int UserId { get; set; }
    public User User { get; set; }
    public int FollowerId { get; set; }
    public User Follower { get; set; }
    
    public Followers(int userId, int followerId)
    {
        UserId = userId;
        FollowerId = followerId;
    }
}