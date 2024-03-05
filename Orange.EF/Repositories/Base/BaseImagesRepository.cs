using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Orange_Bay.Interfaces.Repositories;

namespace Orange.EF.Repositories.Base;

public class BaseImagesRepository<T> : IBaseImagesRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;

    protected BaseImagesRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<int> SaveAsync(T image)
    {
        _context.Set<T>().Add(image);
        return await _context.SaveChangesAsync();
    }

    public Task<T?> FindByAsync(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
    }
}