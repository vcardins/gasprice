using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GasPrice.Core.Common;
using GasPrice.Core.Data.Infrastructure;

namespace GasPrice.Core.Data.Repositories
{
    public interface IQueryFluent<TEntity> where TEntity : IObjectState
    {
        Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, object>> selector = null)
            where TResult : class, new();
        IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression);
        IEnumerable<TEntity> SelectPage(int page, int pageSize);
        Task<IEnumerable<TEntity>> SelectPageAsync(int page, int pageSize);
        Task<PackedList<TResult>> SelectPageAsync<TResult>(int page, int pageSize) where TResult : new();        
        IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector = null);
        IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, object>> selector = null) where TResult : new();
        Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, object>> selector = null) where TResult : new();
        Task<PackedList<TResult>> SelectPagedAsync<TResult>(Expression<Func<TEntity, object>> selector = null) where TResult : new();
        Task<PackedList<TResult>> SelectPagedAsync<TResult>(int page, int pageSize, Expression<Func<TEntity, object>> selector = null) where TResult : new();
        IEnumerable<TEntity> Select();
        Task<IEnumerable<TEntity>> SelectAsync();
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}