using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using GasPrice.Core.Data.DataContext;
using GasPrice.Core.Data.Infrastructure;

namespace GasPrice.Data.EF6
{
    public abstract class DataContext<TContext> : DbContext, IDataContextAsync 
        where TContext : DbContext
    {
        #region Private Fields
        private readonly Guid _instanceId;
        bool _disposed;
        private static bool _databaseInitialized;
        private string _errorMessage = string.Empty;
        private static readonly object Lock = new object();
        #endregion Private Fields

        static DataContext()
        {
            Database.SetInitializer<TContext>(null);
        }

        protected DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            if (_databaseInitialized)
            {
                return;
            }
            lock (Lock)
            {
                if (!_databaseInitialized)
                {
                    _instanceId = Guid.NewGuid();
                    // Set the database intializer which is run once during application start
                    Database.SetInitializer<TContext>(null);
                    _databaseInitialized = true;
                }
            }
        }

        public Guid InstanceId { get { return _instanceId; } }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges"/>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous save operation.  The
        /// <see cref="Task.Result">Task.Result</see> contains the number of
        /// objects written to the underlying database.
        /// </returns>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">A database command did not affect the expected number of rows. This usually
        /// indicates an optimistic concurrency violation; that is, a row has been changed
        /// in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">An attempt was made to use unsupported behavior such as executing multiple
        /// asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">Some error occurred attempting to process entities in the context either
        /// before or after sending commands to the database.</exception>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <seealso cref="DbContext.SaveChangesAsync" />
        public override async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }

        public Task ExecuteSqlAsync(string query, params object[] parameters)
        {
            return Database.ExecuteSqlCommandAsync(query, parameters);
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;           
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
                //var props = dbEntityEntry.Entity.GetProps();
                //var tp = props.GetByName("ObjectState", true);
                //if (tp != null)
                //{
                //    dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
                //}
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}