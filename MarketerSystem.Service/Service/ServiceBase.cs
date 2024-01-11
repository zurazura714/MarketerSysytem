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
            _context = context ?? throw new ArgumentNullException("context");
            if (repository == null) throw new ArgumentNullException("repository");
            _repository = repository;
        }
        public TEntity Fetch(int id)
        {
            return _repository.Fetch(id);
        }

        public IEnumerable<TEntity> Set()
        {
            return _repository.Set();
        }

        public void Save(TEntity entity)
        {
            _repository.Save(entity);
            _context.Commit();
        }
        public void SaveChanges()
        {
            _context.Commit();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _context.Commit();
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);
            _context.Commit();
        }
    }
}
