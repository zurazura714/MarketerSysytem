using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;

namespace MarketerSystem.Service.Service;

public abstract class ServiceBase<TEntity, TRepository> : IServiceBase<TEntity>
    where TEntity : class
    where TRepository : IRepositoryBase<TEntity>
{
    protected readonly IUnitOfWork _context;
    protected readonly TRepository _repository;

    protected ServiceBase(IUnitOfWork context, TRepository repository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public virtual Task<TEntity?> FetchAsync(int id) => _repository.FetchAsync(id);

    public virtual Task<IEnumerable<TEntity>> SetAsync() => _repository.SetAsync();

    public virtual async Task SaveAsync(TEntity entity)
    {
        await _repository.SaveAsync(entity);
        await _context.CommitAsync();
    }

    public virtual Task SaveChangesAsync() => _context.CommitAsync();

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _repository.FetchAsync(id);
        if (entity != null)
        {
            await _repository.DeleteAsync(entity);
            await _context.CommitAsync();
        }
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        await _repository.DeleteAsync(entity);
        await _context.CommitAsync();
    }
}
