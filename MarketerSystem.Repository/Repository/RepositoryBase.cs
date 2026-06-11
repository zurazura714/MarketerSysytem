using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MarketerSystem.Repository.Repository;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly MarketerDBContext _context;

    protected RepositoryBase(IUnitOfWork context)
    {
        _context = context as MarketerDBContext
            ?? throw new ArgumentException($"{nameof(IUnitOfWork)} must be a {nameof(MarketerDBContext)}", nameof(context));
    }

    public virtual async Task<TEntity?> FetchAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public virtual Task<IEnumerable<TEntity>> SetAsync()
    {
        return Task.FromResult<IEnumerable<TEntity>>(_context.Set<TEntity>());
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        var entry = _context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
    }

    public virtual Task SaveAsync(TEntity entity) => AddAsync(entity);

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await FetchAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }

    public virtual Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }
}
