using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GasPrice.Core.Data.Infrastructure;

namespace GasPrice.Core.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IObjectState
    {
        long Count(Expression<Func<TEntity, bool>> query);
        long TotalCount();
        TEntity Create();       
        TEntity Find(params object[] keyValues);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        Task ExecuteSql(string query, params object[] parameters);
        void Insert(TEntity entity, bool? commit = false);
        void InsertRange(IEnumerable<TEntity> entities, bool? commit = false);
        void InsertOrUpdateGraph(TEntity entity, bool? commit = false);
        void InsertGraphRange(IEnumerable<TEntity> entities, bool? commit = false);
        void Update(TEntity entity, bool? commit = false);
        void Delete(object id, bool? commit = false);
        void Delete(TEntity entity, bool? commit = false);
        void Commit();
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        IQueryable<TEntity> Queryable();

        IQueryable<TEntity> QueryByParams(IQueryable<TEntity> queryable,
            IEnumerable<FilterParams> filteringParams = null);

        IQueryable<TEntity> SortByParams(IQueryable<TEntity> queryable,
            IEnumerable<SortParams> sortingParams = null);
        
        IRepository<T> GetRepository<T>() where T : class, IObjectState;
    }
}