using System.Collections.Generic;
using System.Threading.Tasks;
using GasPrice.Core.Models.Modules;

namespace GasPrice.Core.Services
{
    public interface IPriceHistoryService
    {
        Task<IEnumerable<FuelPriceHistory>> GetAll();

    }
}
