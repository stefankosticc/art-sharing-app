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

    public new IEnumerable<User> GetAll(params Expression<Func<User, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<User, object>>>()).ToArray();
        return base.GetAll(combinedIncludes);
    }
    
    public new User GetById(object id, params Expression<Func<User, object>>[] includes)
    {
        var combinedIncludes = DefaultIncludes.Concat(includes ?? Enumerable.Empty<Expression<Func<User, object>>>()).ToArray();
        return base.GetById(id, combinedIncludes);
    }
    
    public IEnumerable<User> GetUsersByName(string name)
    {
        return _dbSet.Include(u => u.Role).Where(u => u.Name.ToLower().Contains(name.ToLower())).ToList();
    }
}
