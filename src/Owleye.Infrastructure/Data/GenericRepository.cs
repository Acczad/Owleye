using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Owleye.Shared.Base;
using Owleye.Shared.Data;

namespace Owleye.Infrastructure.Data
{

    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
            where TEntity : BaseEntity
    {
        protected DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        private IAppSession _appSession { get; }

        public GenericRepository(OwleyeDbContext dbContext, IAppSession appSession)
        {
            Context = dbContext;
            _appSession = appSession;
            DbSet = Context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, dynamic>> includeProperties = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (_appSession.Id.HasValue) { query = DbSet.Where(q => q.CreatedById == _appSession.Id.Value); }


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToArrayAsync();
            }

            return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, dynamic>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (_appSession.Id.HasValue) { query = DbSet.Where(q => q.CreatedById == _appSession.Id.Value); }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Aggregate(query, (current, include) => current.Include(include));

            if (orderBy != null)
            {
                return await orderBy(query).ToArrayAsync();
            }

            return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, dynamic>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = DbSet.Where(filter);
            }

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return orderBy != null ? orderBy(query).FirstOrDefaultAsync() : query.FirstOrDefaultAsync();
        }


        protected IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, dynamic>> includeProperties = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }

            return orderBy != null
                ? orderBy(query)
                : query;
        }

        public TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual TEntity Add(TEntity entity)
        {
            var entry = DbSet.Add(entity);
            return entry.Entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            DbSet.AddRange(entity);
        }

        public virtual TEntity HardDelete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            var entry = DbSet.Remove(entity);
            return entry.Entity;
        }

        public virtual void MarkAsModified(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                DbSet.Attach(entity);

            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> any)
        {
            return DbSet.AnyAsync(any);
        }

        public virtual int SaveChanges()
        {
            return Context.SaveChanges();
        }
        public virtual Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync(true);
        }
    }
}
