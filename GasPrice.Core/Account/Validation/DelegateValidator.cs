/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using GasPrice.Core.Services;

namespace GasPrice.Core.Account.Validation
{
    public class DelegateValidator : IValidator<UserAccount>
    {
        readonly Func<IUserAccountService, UserAccount, string, ValidationResult> func;
        public DelegateValidator(Func<IUserAccountService, UserAccount, string, ValidationResult> func)
        {
            if (func == null) throw new ArgumentNullException("func");

            this.func = func;
        }

        public ValidationResult Validate(IService<UserAccount> service, UserAccount account, string value)
        {
            return func((IUserAccountService) service, account, value);
        }
    }
}
