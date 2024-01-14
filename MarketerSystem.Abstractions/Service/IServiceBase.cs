using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Abstractions.Service
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        Task<TEntity> FetchAsync(int id);
        Task<IEnumerable<TEntity>> SetAsync();
        Task SaveAsync(TEntity entity);
        Task SaveChangesAsync();
        Task DeleteAsync(int id);
        Task DeleteAsync(TEntity entity);
    }
}
