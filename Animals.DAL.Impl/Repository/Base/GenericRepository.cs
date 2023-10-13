using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl.Context;
using Animals.Entities;
using Microsoft.EntityFrameworkCore;

namespace StoreCS.DAL.Impl.Repository.Base;

public abstract class GenericRepository<TKey, TEntity> : IGenericRepository<TKey, TEntity> where TEntity : class
{
    private readonly AnimalsContext _context;

    private DbSet<TEntity> DbSet => _context.Set<TEntity>();

    protected GenericRepository(AnimalsContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var item = await DbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return item.Entity;
    }

    public async Task<bool> DeleteAsync(TKey key)
    {
        TEntity item = await DbSet.FindAsync(key);
        if (item == null)
        {
            return false;
        }

        DbSet.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            return false;
        }

        DbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<List<TEntity>> GetAllAsync(Func<TEntity, bool> predicate)
    {
        List<TEntity> items = await Task.FromResult(DbSet.Where(predicate).ToList());
        return items;
    }

    public virtual async Task<TEntity> GetByIdAsync(TKey key)
    {
        TEntity item = await DbSet.FindAsync(key);
        return item;
    }
    
    public IQueryable<TEntity> GetAllQueryable()
    {
        return (IQueryable<TEntity>)DbSet.AsQueryable();
    }
}
