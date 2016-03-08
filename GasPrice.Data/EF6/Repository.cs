#region

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GasPrice.Core.Data.DataContext;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.Data.UnitOfWork;
using LinqKit;

#endregion

namespace GasPrice.Data.EF6
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields

        private readonly IDataContextAsync _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        #endregion Private Fields

        public Repository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            // Temporarily for FakeDbContext, Unit Test and Fakes
            var dbContext = context as DbContext;

            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
            else
            {
                var fakeContext = context as FakeDbContext;

                if (fakeContext != null)
                {
                    _dbSet = fakeContext.Set<TEntity>();
                }
            }
        }

        public long TotalCount()
        {
            return _dbSet.LongCount();
        }

        public virtual async Task<long> TotalCountAsync()
        {
            return await _dbSet.LongCountAsync();
        }
     
        public long Count(Expression<Func<TEntity, bool>> query)
        {
            return _dbSet.Count(query);
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> query)
        {
            return await _dbSet.CountAsync(query);
        }

        public virtual TEntity Create()
        {
            return _dbSet.Create();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.FirstOrDefault(filter);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public virtual async Task ExecuteSql(string query, params object[] parameters)
        {
            await _context.ExecuteSqlAsync(query, parameters);
        }

        private void _Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            var props = entity.GetType().GetProperties();
            var tp = props.SingleOrDefault(x => x.Name == "Created");
            if (tp != null)
            {
                tp.SetValue(entity, DateTime.Now);
            }
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);

        }

        public virtual void Insert(TEntity entity, bool? commit = false)
        {
            _Insert(entity);
            if (commit.GetValueOrDefault()) { _unitOfWork.SaveChanges(); }
        }

        public async Task InsertAsync(TEntity entity, bool? commit = false)
        {
            _Insert(entity);
            if (commit.GetValueOrDefault()) { await _unitOfWork.SaveChangesAsync(); }
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities, bool? commit = false)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
            if (commit.GetValueOrDefault()) { _unitOfWork.SaveChanges(); }
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities, bool? commit = false)
        {
            _dbSet.AddRange(entities);
            if (commit.GetValueOrDefault()) { _unitOfWork.SaveChanges(); }
        }
        public virtual void InsertOrUpdateGraph(TEntity entity, bool? commit = false)
        {
            SyncObjectGraph(entity);
            _entitesChecked = null;
            _dbSet.Attach(entity);
            if (commit.GetValueOrDefault()) { _unitOfWork.SaveChanges(); }
        }

        private void _Update(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            var props = entity.GetType().GetProperties();
            //TODO: Check whether the property has the AutoPopulate decorator
            var tp = props.SingleOrDefault(x => x.Name == "Updated");
            if (tp != null)
            {
                tp.SetValue(entity, DateTime.Now);
            }
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void Update(TEntity entity, bool? commit = false)
        {
            _Update(entity);
            if (commit.GetValueOrDefault()) { _unitOfWork.SaveChanges(); }
        }

        public async Task UpdateAsync(TEntity entity, bool? commit = false)
        {
            _Update(entity);
            if (commit.GetValueOrDefault()) { await _unitOfWork.SaveChangesAsync(); }
        }

        public virtual void Delete(object id, bool? commit = false)
        {
            var entity = _dbSet.Find(id);
            Delete(entity, commit);
        }

        public virtual void Delete(TEntity entity, bool? commit = false)
        {
            if (entity ==null) return;
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
            if (commit.GetValueOrDefault()) { _unitOfWork.SaveChanges(); }
        }

        public virtual void Commit()
        {
            _unitOfWork.SaveChanges();  
        }

        public virtual async Task<bool> DeleteAsync(bool? commit = false, params object[] keyValues)
        {
            var rsp = await DeleteAsync(CancellationToken.None, keyValues);
            if (commit.GetValueOrDefault()) { await _unitOfWork.SaveChangesAsync(); }
            return rsp;
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);

            return true;
        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        //public virtual IQueryFluent<TEntity> Join(Expression<Func<TEntity, bool>> query)
        //{
        //    return new QueryFluent<TEntity>(this, query);
        //}

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }       

        public IRepository<T> GetRepository<T>() where T : class, IObjectState
        {
            return _unitOfWork.Repository<T>();
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(filter);
            return entity;
        }

        public async Task<TEntity> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> filter = null, List<Expression<Func<TEntity, object>>> includes = null)
        {
            var entries = Select(filter, null, includes);
            var entry = await entries.FirstOrDefaultAsync();
            return entry;
        }

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (orderBy == null) return query;
            query = orderBy(query);
            if (!page.HasValue || !pageSize.HasValue) return query;
            if (page > 0 && pageSize > 0) { 
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal IQueryable<TEntity> Select(
           List<Expression<Func<TEntity, bool>>> filters = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           List<Expression<Func<TEntity, object>>> includes = null,
           int? page = null,
           int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filters != null)
            {
                query = query.AsExpandable();
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));                
            }

            if (orderBy == null) return query;
            query = orderBy(query);
            if (!page.HasValue || !pageSize.HasValue) return query;
            if (page > 0 && pageSize > 0)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null,
            Expression<Func<TEntity, object>> projection = null)
        {
            var entries = Select(filter, orderBy, includes, page, pageSize);
            return await entries.ToListAsync();
        }


        HashSet<object> _entitesChecked; // tracking of all process entities in the object graph when calling SyncObjectGraph

        private void SyncObjectGraph(object entity) // scan object graph for all 
        {
            if(_entitesChecked == null)
                _entitesChecked = new HashSet<object>();

            if (_entitesChecked.Contains(entity))
                return;

            _entitesChecked.Add(entity);

            var objectState = entity as IObjectState;

            if (objectState != null && objectState.ObjectState == ObjectState.Added)
                _context.SyncObjectState((IObjectState)entity);

            // Set tracking state for child collections
            foreach (var prop in entity.GetType().GetProperties())
            {
                // Apply changes to 1-1 and M-1 properties
                var trackableRef = prop.GetValue(entity, null) as IObjectState;
                if (trackableRef != null)
                {
                    if(trackableRef.ObjectState == ObjectState.Added)
                        _context.SyncObjectState((IObjectState) entity);

                    SyncObjectGraph(prop.GetValue(entity, null));
                }

                // Apply changes to 1-M properties
                var items = prop.GetValue(entity, null) as IEnumerable<IObjectState>;
                if (items == null) continue;

                Debug.WriteLine("Checking collection: " + prop.Name);

                foreach (var item in items)
                    SyncObjectGraph(item);
            }
        }

        public IQueryable<TEntity> QueryByParams(IQueryable<TEntity> queryable, IEnumerable<FilterParams> filteringParams = null)
        {
            var tType = typeof(TEntity);
            if (filteringParams == null) return queryable;
            foreach (var param in filteringParams)
            {
                var prop = tType.GetProperty(param.Property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;

                var funcType = typeof(Func<,>).MakeGenericType(tType, prop.PropertyType);

                var lambdaBuilder = typeof(Expression)
                    .GetMethods()
                    .First(x => x.Name == "Lambda" && x.ContainsGenericParameters && x.GetParameters().Length == 2)
                    .MakeGenericMethod(funcType);

                var parameter = Expression.Parameter(tType);
                var propExpress = Expression.Property(parameter, prop);
                var whereLambda = lambdaBuilder.Invoke(null, new object[] { propExpress, new[] { parameter } });

                var where = typeof(Queryable).GetMethods().FirstOrDefault(
                    x => x.Name == "Where" && x.GetParameters().Length == 2);
                if (@where == null) continue;

                @where = @where.MakeGenericMethod(tType, prop.PropertyType);
                queryable = (IOrderedQueryable<TEntity>)@where.Invoke(null, new[] { queryable, whereLambda });
            }
            return queryable;
        }

        public IQueryable<TEntity> SortByParams(IQueryable<TEntity> query, IEnumerable<SortParams> sortingParams = null)
        {
            var tType = typeof(TEntity);
            MethodInfo sorter = null;
            if (sortingParams != null)
            {
                foreach (var param in sortingParams)
                {
                    var isDescending = param.Direction.ToLower().Contains("desc");
                    var prop = tType.GetProperty(param.Property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop == null) continue;

                    var funcType = typeof(Func<,>).MakeGenericType(tType, prop.PropertyType);

                    var lambdaBuilder = typeof(Expression)
                        .GetMethods()
                        .First(x => x.Name == "Lambda" && x.ContainsGenericParameters && x.GetParameters().Length == 2)
                        .MakeGenericMethod(funcType);

                    var parameter = Expression.Parameter(tType);
                    var propExpress = Expression.Property(parameter, prop);
                    var sortLambda = lambdaBuilder.Invoke(null, new object[] { propExpress, new[] { parameter } });

                    sorter = typeof(Queryable).GetMethods().FirstOrDefault(
                        x =>
                        x.Name ==
                        (sorter == null
                                ? (isDescending ? "OrderByDescending" : "OrderBy")
                                : (isDescending ? "ThenByDescending" : "ThenBy")) && x.GetParameters().Length == 2);
                    if (sorter == null) continue;

                    sorter = sorter.MakeGenericMethod(new[] { tType, prop.PropertyType });
                    query = (IOrderedQueryable<TEntity>)sorter.Invoke(null, new[] { query, sortLambda });
                }
            }
            return query;
        }

    }
}