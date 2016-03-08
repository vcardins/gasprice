using System.Threading.Tasks;
using GasPrice.Core.ViewModels;

namespace GasPrice.Core.Services
{
    public interface ILookupService
    {
        Task<Lookups> GetAll();

    }
}
