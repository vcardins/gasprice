using System.Threading;
using System.Threading.Tasks;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.Repositories;

namespace GasPrice.Core.Data.UnitOfWork
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState;
    }
}