using System.Collections.Generic;
using System.Threading.Tasks;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Models;
using GasPrice.Core.ViewModels;

namespace GasPrice.Core.Services
{
    public interface IAppSettingsService : IService<AppSettings>
    {
        Task<AppSettingsOutput> GetAppSettings();

        Task<ModelAction> SaveSettings(AppSettingsInput settings, IDictionary<string, object> fields = null);

    }
}
