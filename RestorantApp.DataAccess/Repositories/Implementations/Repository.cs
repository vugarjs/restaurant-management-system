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
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);

        return;
    }
    public void Remove(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;
        if (disableTracking)
            query = query.AsNoTracking();

        // Əgər include verilibsə, sorğuya əlavə edirik
        if (include != null)
            query = include(query);

        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

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


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<T> FindSingleAsync(Expression<Func<T, bool>>? predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate!)!;
    }
}

