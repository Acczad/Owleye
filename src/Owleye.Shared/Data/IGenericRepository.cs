using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Owleye.Shared.Data
{
    public interface IGenericRepository<TEntity> where TEntity : IBaseEntity
    {

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>> include = null);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, dynamic>>[] includeProperties);

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, dynamic>>[] includes);


        TEntity GetById(object id);
        TEntity Add(TEntity entity);
        void MarkAsModified(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> any);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
