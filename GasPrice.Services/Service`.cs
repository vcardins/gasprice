using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Common;
using App.Core.Data;
using App.Core.EventHandling;
using App.Core.Models;
using App.Core.Services;
using App.Services.EventHandling;
using Omu.ValueInjecter;

namespace App.Services
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : BaseEntity
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        protected readonly IRepository<TEntity> Repository;
        private bool _disposed;
        protected EventsHandler EventsHandler { get; set; }

        protected Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repository = UnitOfWork.Repository<TEntity>();
            EventsHandler = new EventsHandler();
        }

        protected readonly List<IEvent> Events = new List<IEvent>();

        IEnumerable<IEvent> IEventSource.GetEvents()
        {
            return Events;
        }
        void IEventSource.Clear()
        {
            Events.Clear();
        }
        //public EventsHandler EventsHandler { get; set; }

        public void ClearEvents()
        {
            Events.Clear();
        }

        public void AddEvent(IEvent evt)
        {
            if (evt is IAllowMultiple || Events.All(x => x.GetType() != evt.GetType()))
            {
                Events.Add(evt);
            }
        }

        public void RaiseEvent(IEvent evt)
        {
            //if (evt is IAllowMultiple)
            //{
                EventsHandler.EventBus.RaiseEvent(evt);
            //}
        }

        public void FireEvents()
        {
            foreach (var ev in Events)
            {
                var ev1 = ev;
                EventsHandler.EventBus.RaiseEvent(ev1);
            }
        }

        public int Count()
        {
            return Repository.Count();
        }

        public List<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

        public PaginatedList<TEntity> GetAll(int pageIndex, int pageSize)
        {
            return Repository.GetAll(pageIndex, pageSize);
        }

        public PaginatedList<TEntity> GetAll(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderSelector)
        {
            return Repository.GetAll(pageIndex, pageSize, orderSelector);
        }

        public PaginatedList<TEntity> GetAll(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderSelector, Expression<Func<TEntity, bool>> predicate,  params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.GetAll(pageIndex, pageSize, orderSelector, predicate, includeProperties);
        }

        public TEntity GetById(params object[] keyValues)
        {
            return Repository.GetSingle(keyValues);
        }

        public Task<TEntity> GetByIdIncluding(object[] keyValues, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.GetSingleIncludingAsync(keyValues, includeProperties);
        }

        public void Add(TEntity entity)
        {
            Repository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        
        public void Update(TEntity entity)
        {
            Repository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            Repository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        public Task DeleteById(params object[] keyValues)
        {
            var entity = GetById(keyValues);
            Repository.Delete(entity);
            UnitOfWork.SaveChanges();
            return Task.FromResult(0);
        }

        public TEntity Clone(Expression<Func<TEntity, bool>> filter)
        {
            return Repository.Clone(filter);
        }

        public Task<int> CountAsync()
        {
            return Repository.CountAsync();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return Repository.GetAllAsync();
        }

        public Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize)
        {
            return Repository.GetAllAsync(pageIndex, pageSize);
        }

        public Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderSelector)
        {
            return Repository.GetAllAsync(pageIndex, pageSize, orderSelector);
        }

        public Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderSelector, Expression<Func<TEntity, bool>> predicate,  params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Repository.GetAllAsync(pageIndex, pageSize, orderSelector, predicate, includeProperties);
        }

        public Task<List<TViewModel>> GetAllAsync<TViewModel>() where TViewModel : new()
        {
            var records = Repository.GetAll();
            var result = records.Select(x => new TViewModel().InjectFrom(x)).Cast<TViewModel>().ToList();
            return Task.FromResult(result);
        }

        public async Task<PaginatedList<TViewModel>> GetAllAsync<TViewModel>(int pageIndex, int pageSize) where TViewModel : new()
        {
            var records = await Repository.GetAllAsync(pageIndex, pageSize, y => y.OrderBy(z => z.Created));
            var result = records.Select(x => new TViewModel().InjectFrom(x)).Cast<PaginatedList<TViewModel>>();
            throw new NotImplementedException();
            //return result;
        }

        public Task<PaginatedList<TViewModel>> GetAllAsync<TViewModel>(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy) where TViewModel : new()
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<TViewModel>> GetAllAsync<TViewModel>(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderSelector, Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties) where TViewModel : new()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(params object[] keyValues)
        {
            return Repository.GetSingleAsync(keyValues);
        }

        public Task AddAsync(TEntity entity)
        {
            Repository.Insert(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            Repository.Update(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            Repository.Delete(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        public IQueryable<TEntity> Queryable()
        {
            return Repository.Queryable();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Query(query, null, includeProperties);
        }

        public IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Query(null, orderBy, includeProperties);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Expression<Func<TEntity, object>>[] includeProperties, int pageSize = 0)
        {
            return Repository.Query(query, orderBy, includeProperties, pageSize);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                UnitOfWork.Dispose();
            }
            _disposed = true;
        }
    }
}
