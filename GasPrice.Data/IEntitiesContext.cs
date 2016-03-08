using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Data
{
    public interface IEntitiesContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : Entity;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : Entity;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : Entity;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : Entity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        int Commit();
        void Rollback();
        Task<int> CommitAsync();
        ObjectContext ObjectContext();
    }
}