using System.Collections.Generic;
using System.Threading.Tasks;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Models.Modules;
using GasPrice.Core.Services;

namespace GasPrice.Services.Services
{
    public class PriceHistoryService : Service<FuelPriceHistory>, IPriceHistoryService
    {
        private readonly IUnitOfWorkAsync _unitOfWork;

        public PriceHistoryService(IUnitOfWorkAsync unitOfWork)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


      
        public async Task<IEnumerable<FuelPriceHistory>> GetAll()
        {
            var all = await Repository.Query().SelectAsync();
            return all;
        }
      
    }

}
