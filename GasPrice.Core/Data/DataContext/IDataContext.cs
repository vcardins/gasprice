using System;
using GasPrice.Core.Data.Infrastructure;

namespace GasPrice.Core.Data.DataContext
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;
        void SyncObjectsStatePostCommit();
    }
}