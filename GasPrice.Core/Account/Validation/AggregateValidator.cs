using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Services;

namespace GasPrice.Core.Account.Validation
{
    public class AggregateValidator<T> : List<IValidator<T>>, IValidator<T> 
        where T : IObjectState
    {
        public ValidationResult Validate(IService<T> service, T account, string value)
        {
            if (service == null) throw new ArgumentNullException("service");
            if (account == null) throw new ArgumentNullException("account");
            
            foreach (var item in this)
            {
                var result = item.Validate(service, account, value);
                if (result != null && result != ValidationResult.Success)
                {
                    return result;
                }
            }
            return null;
        }

    }
}
