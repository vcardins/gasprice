using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GasPrice.Core.Common;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.EventHandling;

namespace GasPrice.Core.Services
{
    public interface IService<TEntity> : IEventSource where TEntity : IObjectState
    {
        int CurrentUserId { get; set; }
        long TotalCount();
        TEntity Find(params object[] keyValues);
        void Insert(TEntity entity, bool? commit = false);
        void InsertRange(IEnumerable<TEntity> entities, bool? commit = false);
        void InsertOrUpdateGraph(TEntity entity, bool? commit = false);
        void InsertGraphRange(IEnumerable<TEntity> entities, bool? commit = false);
        void Update(TEntity entity, bool? commit = false);
        void Delete(object id, bool? commit = false);
        void Delete(TEntity entity, bool? commit = false);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        int Commit();
        IQueryFluent<TEntity> Query();
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<long> TotalCountAsync();
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
        IQueryable<TEntity> Queryable();
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(int? pageIndex, int? pageSize) 
            where TViewModel : new();
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int? pageIndex, int? pageSize) 
            where TViewModel : new();
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, 
            Expression<Func<TEntity, object>> includeProperties, int? pageIndex = null, int? pageSize = null) 
            where TViewModel : new();

        Task<PackedList<TViewModel>> SelectAsync<TViewModel>(int? pageIndex, int? pageSize)
         where TViewModel : new();

        Task<PackedList<TViewModel>> SelectAsync<TViewModel>(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, object>> includeProperties = null, 
            int? pageIndex = null, int? pageSize = null) 
            where TViewModel : new();

        void AddEvent(IEvent evt);
        void RaiseEvent(IEvent evt);
        void FireEvents();
        void ClearEvents();
        string GetValidationMessage(string id);
        void AddEventHandler(params IEventHandler[] handlers);
    }

}