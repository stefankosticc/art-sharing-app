using Microsoft.EntityFrameworkCore;

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

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public T GetById(object id)
    {
        return _dbSet.Find(id);
    }

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
}