using System.Linq.Expressions;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IGenericRepository<T> where T : class
{
    // IEnumerable<T> GetAll();
    IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
    // T GetById(object id);
    T GetById(object id, params Expression<Func<T, object>>[] includes);
    void Add(T obj);
    void Update(T obj);
    void Delete(object id);
    void Save();

}