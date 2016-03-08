using System.Threading;
using System.Threading.Tasks;

namespace GasPrice.Core.Data.DataContext
{
    public interface IDataContextAsync : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
        Task ExecuteSqlAsync(string query, params object[] parameters);
    }
}