using Microsoft.EntityFrameworkCore;
using RestorantApp.DataAccess.Context;
using RestorantApp.DataAccess.Repositories.Interfaces;
using RestorantApp.Entity.Entities.Common;
using System.Linq.Expressions;

namespace RestorantApp.DataAccess.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly RestorantContext _context;
    protected readonly DbSet<T> _dbSet;
    public Repository(RestorantContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Remove(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
    public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>>? predicate)
    {
        return await _dbSet.Where(predicate!).ToListAsync();
    }

    public Task<T> Get(Expression<Func<T, bool>>? predicate)
    {
        return _dbSet.FirstOrDefaultAsync(predicate!)!;
    }
}

