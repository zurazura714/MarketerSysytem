using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Abstractions.Service
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        TEntity Fetch(int id);
        IEnumerable<TEntity> Set();
        void Save(TEntity entity);
        void SaveChanges();
        void Delete(int id);
        void Delete(TEntity entity);
    }
}
