namespace MarketerSystem.Abstractions.Repository;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity?> FetchAsync(int id);

    /// <summary>
    /// Composable query root. Filters, Includes, and paging applied by callers
    /// translate to SQL — nothing materializes until the query is enumerated.
    /// </summary>
    IQueryable<TEntity> Set();

    Task SaveAsync(TEntity entity);
    Task DeleteAsync(int id);
    Task DeleteAsync(TEntity entity);
}
