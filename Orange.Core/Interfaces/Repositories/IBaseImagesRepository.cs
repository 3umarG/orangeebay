using System.Linq.Expressions;

namespace Orange_Bay.Interfaces.Repositories;

public interface IBaseImagesRepository<T> where T : class
{
    Task<int> SaveAsync(T image);
    Task<T?> FindByAsync(Expression<Func<T, bool>> predicate);
}