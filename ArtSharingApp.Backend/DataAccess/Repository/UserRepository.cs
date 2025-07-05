using System.Linq.Expressions;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    protected Expression<Func<User, object>>[] DefaultIncludes => new Expression<Func<User, object>>[]
    {
        u => u.Role
    };
    
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetAllAsync(params Expression<Func<User, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<User, object>>>()).ToArray();
        return await base.GetAllAsync(combinedIncludes);
    }
    
    public async Task<User> GetByIdAsync(object id, params Expression<Func<User, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<User, object>>>()).ToArray();
        return await base.GetByIdAsync(id, combinedIncludes);
    }

    public void UpdateBiography(User user)
    {
        _context.Attach(user);
        _context.Entry(user).Property(u => u.Biography).IsModified = true;
    }

    public async Task<IEnumerable<User>> GetUsersByNameAndUserName(string searchString)
    {
        return await _dbSet
            .Where(u => u.UserName.ToLower().Contains(searchString.ToLower())
                        || u.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
    }
}
