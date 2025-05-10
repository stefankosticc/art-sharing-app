using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public IEnumerable<User> GetUsersByName(string name)
    {
        return _dbSet.Where(u => u.Name.ToLower().Contains(name.ToLower())).ToList();
    }
}
