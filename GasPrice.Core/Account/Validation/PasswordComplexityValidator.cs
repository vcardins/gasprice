/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GasPrice.Core.Constants;
using GasPrice.Core.Services;

namespace GasPrice.Core.Account.Validation
{
    public class PasswordComplexityValidator : IValidator<UserAccount>
    {
        public int MinimumLength { get; set; }
        public int MinimumNumberOfComplexityRules { get; set; }

        public PasswordComplexityValidator()
            : this(UserAccountConstants.PasswordComplexity.MinimumLength, UserAccountConstants.PasswordComplexity.NumberOfComplexityRules)
        {
        }

        public PasswordComplexityValidator(int minimumLength, int minimumNumberOfComplexityRules)
        {
            if (minimumLength <= 0) minimumLength = 1;
            MinimumLength = minimumLength;
            
            if (minimumNumberOfComplexityRules < 0) minimumNumberOfComplexityRules = 0;
            if (minimumNumberOfComplexityRules > 4) MinimumNumberOfComplexityRules = 4;
            MinimumNumberOfComplexityRules = minimumNumberOfComplexityRules;
        }

        public ValidationResult Validate(IService<UserAccount> service, UserAccount account, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.PasswordRequired));
            }
            
            if (value.Length < MinimumLength)
            {
                return new ValidationResult(String.Format(service.GetValidationMessage(UserAccountConstants.ValidationMessages.PasswordLength), MinimumLength));
            }

            var upper = value.Any(Char.IsUpper);
            var lower = value.Any(Char.IsLower);
            var digit = value.Any(Char.IsDigit);
            var other = value.Any(x => !Char.IsUpper(x) && !Char.IsLower(x) && !Char.IsDigit(x));

            var vals = new[] { upper, lower, digit, other };
            var matches = vals.Where(x => x).Count();
            if (matches < MinimumNumberOfComplexityRules)
            {
                return new ValidationResult(String.Format(service.GetValidationMessage(UserAccountConstants.ValidationMessages.PasswordComplexityRules), MinimumNumberOfComplexityRules));
            }

            return null;
        }
    }
}
