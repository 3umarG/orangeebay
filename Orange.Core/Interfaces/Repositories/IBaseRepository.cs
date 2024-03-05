using System.Linq.Expressions;

namespace Orange_Bay.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> FindAllAsync();
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindAllAsync(IEnumerable<string> includes);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, IEnumerable<string> includes);

    Task<T?> FindByIdAsync(int id);
    Task<T?> FindByPredicateAsync(Expression<Func<T, bool>> predicate, IEnumerable<string> includes);

    Task<T?> DeleteByIdAsync(int id);

    Task<T?> SaveAsync(T entity);
    Task<int?> AddAsync(T entity);
    IQueryable<T> QueryableOf();
}