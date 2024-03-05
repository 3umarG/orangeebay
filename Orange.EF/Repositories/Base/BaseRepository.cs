using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Repositories;

namespace Orange.EF.Repositories.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync(IEnumerable<string> includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        var result = await query.ToListAsync();
        return result;
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, IEnumerable<string> includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        var result = await query.Where(predicate).ToListAsync();
        return result;
    }

    public async Task<T?> FindByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> FindByPredicateAsync(Expression<Func<T, bool>> predicate, IEnumerable<string> includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> DeleteByIdAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        if (entity is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Resource with id : {id}");
        }

        var removedEntity = _dbContext.Remove(entity).Entity;
        await _dbContext.SaveChangesAsync();
        return removedEntity;
    }

    public async Task<T?> SaveAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<int?> AddAsync(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public IQueryable<T> QueryableOf()
    {
        return _dbContext.Set<T>();
    }
}