using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Services;
using GasPrice.Core.ViewModels;

namespace GasPrice.Services.Services
{
    public class LookupService : ILookupService
    {
        private readonly IDataCache _cache;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public LookupService(IUnitOfWorkAsync unitOfWork, IDataCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }


        public IEnumerable<EnumOutput> GetEnumOutput(Enum e)
        {
            var values = Enum.GetValues(e.GetType()).Cast<object>();
            var models = values.Select(v => new EnumOutput { Id = (int)v, Name = v.ToString() }).ToList();
            return models;
        }

        public async Task<Lookups> GetAll()
        {
            if (!_cache.Contains(DataCacheKey.Lookups))
            {
                var models = new Lookups
                {
                    Genders = GetEnumOutput(Gender.Female),
                };
                //return await Task.Run(() => models);
                _cache.Insert(DataCacheKey.Lookups, models);
            }
            return await Task.Run(() => _cache.Get<Lookups>(DataCacheKey.Lookups));
            //var models = _cache.Get<Lookups>(DataCacheKey.Lookups);
            //return await Task.Run(() => models);
        }
      
    }

}
