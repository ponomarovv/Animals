namespace Animals.DAL.Abstract.Repository.Base;

public interface IGenericRepository <TKey,TEntity>
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<bool> DeleteAsync(TKey key);
    Task<bool> UpdateAsync(TEntity entity);
    Task<List<TEntity>> GetAllAsync(Func<TEntity, bool> predicate);
    Task<TEntity> GetByIdAsync(TKey key);
    public IQueryable<TEntity> GetAllQueryable();
}
