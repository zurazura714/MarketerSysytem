using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Repository.Repository
{

    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected MarketerDBContext _context;

        public IUnitOfWork Context
        {
            get { return _context; }
        }

        internal RepositoryBase(IUnitOfWork context) : this(context as MarketerDBContext) { }

        internal RepositoryBase(MarketerDBContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        public virtual async Task<TEntity> FetchAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> SetAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await AddAsync(_context.Set<TEntity>(), entity);
        }

        public virtual async Task SaveAsync(TEntity entity)
        {
            await SaveAsync(_context.Set<TEntity>(), entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await FetchAsync(id);
            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await DeleteAsync(_context.Set<TEntity>(), entity);
        }

        protected virtual async Task SaveAsync(DbSet<TEntity> set, TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry == null || entry.State == EntityState.Detached)
            {
                await set.AddAsync(entity);
            }
        }

        protected virtual async Task DeleteAsync(DbSet<TEntity> set, TEntity entity)
        {
            set.Remove(entity);
        }

        protected virtual async Task AddAsync(DbSet<TEntity> set, TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry == null || entry.State == EntityState.Detached)
            {
                await set.AddAsync(entity);
            }
        }
    }
}