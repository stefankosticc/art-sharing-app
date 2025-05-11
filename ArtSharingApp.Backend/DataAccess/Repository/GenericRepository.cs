using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext _context;
    protected DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // public IEnumerable<T> GetAll()
    // {
    //     return _dbSet.ToList();
    // }
    
    public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return query.ToList();
    }

    // public T GetById(object id)
    // {
    //     return _dbSet.Find(id);
    // }

    public void Add(T obj)
    {
        _dbSet.Add(obj);
    }

    public void Update(T obj)
    {
        _dbSet.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(object id)
    {
        T existing = _dbSet.Find(id);
        if (existing != null)
        {
            _dbSet.Remove(existing);
        }
    }
    
    public void Save()
    {
        _context.SaveChanges();
    }

    public T GetById(object id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        // Assumes the key is named "Id"
        return query.FirstOrDefault(e => EF.Property<object>(e, "Id").Equals(id));
    }
}
