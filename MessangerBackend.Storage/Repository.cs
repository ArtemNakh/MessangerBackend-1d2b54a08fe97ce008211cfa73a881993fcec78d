
using MessangerBackend.Core.Interfaces;
using MessangerBackend.Storage;
using System.Linq.Expressions;

public class Repository : IRepository
{
    private readonly MessangerContext _context;

    public Repository(MessangerContext context)
    {
        _context = context;
    }

    public async Task<T> Add<T>(T entity) where T : class
    {
        var obj = _context.Add(entity);
        await _context.SaveChangesAsync();
        return obj.Entity;
    }

    public async Task<T> Update<T>(T entity) where T : class
    {
        var newEntity = _context.Update(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task<T> Delete<T>(int id) where T : class
    {
        var entity = await GetById<T>(id);
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> GetById<T>(int id) where T : class
    {
        return await _context.Set<T>().FindAsync(id).AsTask();
    }

    public IQueryable GetAll<T>() where T : class
    {
       return _context.Set<T>();
    }
    
    public async Task<IEnumerable<T>> GetQuery<T>(Expression<Func<T, bool>> func) where T : class
    {
        return _context.Set<T>().Where(func);
    }
}