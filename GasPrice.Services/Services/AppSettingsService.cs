using System.Collections.Generic;
using System.Threading.Tasks;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Common.Extensions;
using GasPrice.Core.Common.Information;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Models;
using GasPrice.Core.Services;
using GasPrice.Core.ViewModels;
using Omu.ValueInjecter;

namespace GasPrice.Services.Services
{
    public class AppSettingsService : Service<AppSettings>, IAppSettingsService
    {
        private readonly AppSettings _defaultSettings;

        public AppSettingsService(IUnitOfWorkAsync unitOfWork, IApplicationInformation appInfo)
            : base(unitOfWork)
        {
            _defaultSettings = new AppSettings
            {
                ShortName = appInfo.ApplicationName,
                LongName = appInfo.ApplicationName,
                Description = appInfo.ApplicationName,
                Copyright = appInfo.ApplicationName,
                Keywords = null,
                Version = null,
                Email = appInfo.Email,
                EmailSignature = appInfo.ApplicationName,
                LongDateTimeFormat = null,
                ShortDateTimeFormat = null,
                TinyDateTimeFormat = null,
                GoogleAnalyticsKey = null
            };
        }

        public async Task<AppSettingsOutput> GetAppSettings()
        {
            var entity = await FirstOrDefaultAsync(e => true);
            var result = entity ?? _defaultSettings;
            return new AppSettingsOutput().InjectFrom(result) as AppSettingsOutput;
        }

        public async Task<ModelAction> SaveSettings(AppSettingsInput model, IDictionary<string, object> fields = null)
        {
            var entity = await FirstOrDefaultAsync(e => true);
            entity = entity ?? _defaultSettings;

            if (model != null)
            {
                entity.InjectFrom(model);
            }
            else
            {
                entity.InjectFrom(new DictionaryInjection(new[] { "Id" }), fields);
            }

            var action = ModelAction.Create;
            if (entity.Id == 0)
            {
                Repository.Insert(entity, true);
            }
            else
            {
                Repository.Update(entity, true);
                action = ModelAction.Update;
            }
            return action;
        }
    }
    
}
