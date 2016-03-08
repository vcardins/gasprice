using System.Threading.Tasks;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Models.Infraestructure;
using GasPrice.Core.Services;

namespace GasPrice.Services.Services
{
    public class ExceptionLogService : Service<ExceptionLog>, IExceptionLogService
    {
        public ExceptionLogService(IUnitOfWorkAsync unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task Store(ExceptionLog err)
        {
            await Repository.InsertAsync(err, true);
        }
    }
    
}
