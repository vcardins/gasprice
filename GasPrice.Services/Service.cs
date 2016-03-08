using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GasPrice.Core.Account.Events;
using GasPrice.Core.Common;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.EventHandling;
using GasPrice.Core.Services;
using Omu.ValueInjecter;

namespace GasPrice.Services
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields
        public int CurrentUserId { get; set; }
        protected IUnitOfWorkAsync UnitOfWork { get; private set; }
        protected readonly IRepositoryAsync<TEntity> Repository;
        protected EventsHandler EventsHandler { get; set; }
        #endregion Private Fields

        #region Constructor

        protected Service(IUnitOfWorkAsync unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repository = UnitOfWork.RepositoryAsync<TEntity>();
            EventsHandler = new EventsHandler();
        }
        #endregion Constructor

        protected readonly List<IEvent> Events = new List<IEvent>();
        protected readonly CommandBus CommandBus = new CommandBus();
        

        IEnumerable<IEvent> IEventSource.GetEvents()
        {
            return Events;
        }
        void IEventSource.Clear()
        {
            Events.Clear();
        }

        public void ClearEvents()
        {
            Events.Clear();
        }

        public void AddEventHandler(params IEventHandler[] handlers)
        {
            //foreach (var h in handlers) VerifyHandler(h);
            EventsHandler.AddEventHandler(handlers);
        }

        public void AddCommandHandler(ICommandHandler handler)
        {
            CommandBus.Add(handler);
        }
        
        public void ExecuteCommand(ICommand cmd)
        {
            CommandBus.Execute(cmd);
            //Configuration.CommandBus.Execute(cmd);
        }

        public string GetValidationMessage(string id)
        {
            var cmd = new GetValidationMessage { ID = id };
            ExecuteCommand(cmd);
            if (cmd.Message != null) return cmd.Message;

            var result = Resources.ValidationMessages.ResourceManager.GetString(id, Resources.ValidationMessages.Culture);
            if (result == null) throw new Exception("Missing validation message for ID : " + id);
            return result;
        }

        public void AddEvent(IEvent evt)
        {
            //evt is IAllowMultiple || 
            if (Events.All(x => x.GetType() != evt.GetType()))
            {
                Events.Add(evt);
            }
        }

        public void RaiseEvent(IEvent evt)
        {
            //if (evt is IAllowMultiple)
            EventsHandler.EventBus.RaiseEvent(evt);
        }

        public void FireEvents()
        {
            foreach (var ev in Events)
            {
                var ev1 = ev;
                EventsHandler.EventBus.RaiseEvent(ev1);
            }
        }

        public long TotalCount() { return Repository.TotalCount(); }

        public virtual async Task<long> TotalCountAsync() { return await Repository.TotalCountAsync(); }

        public virtual TEntity Find(params object[] keyValues) { return Repository.Find(keyValues); }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return Repository.SelectQuery(query, parameters).AsQueryable();             
        }

        public virtual void Insert(TEntity entity, bool? commit = false)
        {
            Repository.Insert(entity, commit);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities, bool? commit = false)
        {
            Repository.InsertRange(entities, commit);
        }

        public virtual void InsertOrUpdateGraph(TEntity entity, bool? commit = false)
        {
            Repository.InsertOrUpdateGraph(entity, commit);
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities, bool? commit = false)
        {
            Repository.InsertGraphRange(entities, commit);
        }

        public virtual void Update(TEntity entity, bool? commit = false)
        {
            Repository.Update(entity, commit);
        }

        public virtual void Delete(object id, bool? commit = false)
        {
            Repository.Delete(id, commit);
        }

        public virtual void Delete(TEntity entity, bool? commit = false)
        {
            Repository.Delete(entity, commit);
        }

        public virtual int Commit() { return UnitOfWork.SaveChanges(); }

        public IQueryFluent<TEntity> Query() { return Repository.Query(); }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return Repository.Query(queryObject);
        }
        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return Repository.Query(query);
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await Repository.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await Repository.FindAsync(cancellationToken, keyValues);
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Count(predicate);
        }
        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Repository.CountAsync(predicate);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.FirstOrDefault(predicate);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Repository.FirstOrDefaultAsync(predicate);
        }  
        
        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        //IF 04/08/2014 - Before: return await DeleteAsync(cancellationToken, keyValues);
        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await Repository.DeleteAsync(cancellationToken, keyValues);
        }

        public IQueryable<TEntity> Queryable() { return Repository.Queryable(); }

        public virtual async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(int? pageIndex, int? pageSize) 
            where TViewModel : new()
        {
            return await GetAllAsync<TViewModel>(null, null, null, pageIndex, pageSize);
        }

        public virtual async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, 
            int? pageIndex, int? pageSize) 
            where TViewModel : new()
        {
            return await GetAllAsync<TViewModel>(null, orderBy, null, pageIndex, pageSize);
        }

        public virtual async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Expression<Func<TEntity, object>> includeProperties, 
            int? pageIndex, int? pageSize) 
            where TViewModel : new()
        {
            var query = await Query(predicate).Include(includeProperties).OrderBy(orderBy).SelectAsync();
            var result = query.Select(x => new TViewModel().InjectFrom(x)).Cast<TViewModel>();
            return result;
        }

        public virtual async Task<PackedList<TViewModel>> SelectAsync<TViewModel>(int? pageIndex, int? pageSize)
          where TViewModel : new()
        {
            return await SelectAsync<TViewModel>(null, null, null, pageIndex, pageSize);
        }

        public virtual async Task<PackedList<TViewModel>> SelectAsync<TViewModel>(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Expression<Func<TEntity, object>> includeProperties,
            int? pageIndex, int? pageSize)
           where TViewModel : new()
        {
            var query = await Query(predicate).Include(includeProperties).OrderBy(orderBy).SelectPageAsync(pageIndex ?? 0, pageSize ?? 0);
            
            var result = new PackedList<TViewModel>
            {
                Data = query.Select(x => new TViewModel().InjectFrom(x)).Cast<TViewModel>(),
                Total = TotalCount()
            };

            return result;
        }
    }
}