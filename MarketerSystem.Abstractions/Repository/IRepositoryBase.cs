namespace MarketerSystem.Abstractions.Repository;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity?> FetchAsync(int id);
    Task<IEnumerable<TEntity>> SetAsync();
    Task AddAsync(TEntity entity);
    Task SaveAsync(TEntity entity);
    Task DeleteAsync(int id);
    Task DeleteAsync(TEntity entity);
}
