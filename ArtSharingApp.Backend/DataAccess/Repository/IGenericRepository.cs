namespace ArtSharingApp.Backend.DataAccess.Repository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(object id);
    void Add(T obj);
    void Update(T obj);
    void Delete(object id);
    void Save();
}