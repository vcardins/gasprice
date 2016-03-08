using System.Threading.Tasks;
using GasPrice.Core.Models.Infraestructure;

namespace GasPrice.Core.Services
{
    public interface IExceptionLogService : IService<ExceptionLog>
    {
        Task Store(ExceptionLog err);
    }
}
