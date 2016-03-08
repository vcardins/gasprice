using System;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.Repositories;

namespace GasPrice.Core.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        void BeginTransaction(DbIsolationLevel isolationLevel = DbIsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}