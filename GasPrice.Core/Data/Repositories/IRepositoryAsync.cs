using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GasPrice.Core.Data.Infrastructure;

namespace GasPrice.Core.Data.Repositories
{
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : class, IObjectState
    {
        Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);
        Task<long> TotalCountAsync();
        Task<TEntity> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> filter, List<Expression<Func<TEntity, object>>> includes = null);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task InsertAsync(TEntity entity, bool? commit = false);
        Task UpdateAsync(TEntity entity, bool? commit = false);
        Task<bool> DeleteAsync(bool? commit = false, params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);

    }
}