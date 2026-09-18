using RestorantApp.Entity.Entities.Common;
using System.Linq.Expressions;

namespace RestorantApp.DataAccess.Repositories.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<List<T>> FindAsync(Expression<Func<T, bool>>? predicate);
    Task<T> FindSingleAsync(Expression<Func<T, bool>>? predicate);
    Task AddAsync(T entity);
    void Remove(int id);
    Task<List<T>> GetAllAsync();
    Task<T> Get(Expression<Func<T, bool>>? predicate);
    void Update(T entity);
    Task<int> SaveChangesAsync();
}
