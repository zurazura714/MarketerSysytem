using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Service.Service
{
    public abstract class ServiceBase<TEntity, TRepository> : IServiceBase<TEntity>
    where TEntity : class
    where TRepository : IRepositoryBase<TEntity>
    {
        protected IUnitOfWork _context;
        protected TRepository _repository;

        internal ServiceBase(IUnitOfWork context, TRepository repository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public virtual async Task<TEntity> FetchAsync(int id)
        {
            return await _repository.FetchAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> SetAsync()
        {
            return await _repository.SetAsync();
        }

        public virtual async Task SaveAsync(TEntity entity)
        {
            await _repository.SaveAsync(entity);
            await _context.CommitAsync();
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.CommitAsync();
        }

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
}
