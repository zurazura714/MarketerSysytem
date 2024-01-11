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

        void Add(TEntity entity);

        TEntity Fetch(int id);

        IEnumerable<TEntity> Set();

        void Save(TEntity entity);

        void Delete(int id);

        void Delete(TEntity entity);
    }
}
