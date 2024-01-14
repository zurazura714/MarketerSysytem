using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Abstractions.Repository
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IUnitOfWork Context { get; }

        Task AddAsync(TEntity entity);

        Task<TEntity> FetchAsync(int id);

        Task<IEnumerable<TEntity>> SetAsync();

        Task SaveAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task DeleteAsync(TEntity entity);
    }
}
