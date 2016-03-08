/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Services;

namespace GasPrice.Core.Account.Validation
{
    public interface IValidator<T> where T : IObjectState
    {
        ValidationResult Validate(IService<T> service, T account, string value);
    }
}
    