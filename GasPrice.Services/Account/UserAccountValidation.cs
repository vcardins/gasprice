/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GasPrice.Core.Account;
using GasPrice.Core.Account.Validation;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Constants;

namespace GasPrice.Services.Account
{
    internal class UserAccountValidation
    {
        public static readonly IValidator<UserAccount> UsernameDoesNotContainAtSign =
            new DelegateValidator((service, account, value) =>
            {
                if (value.Contains("@"))
                {
                    Tracing.Verbose("[UserAccountValidation.UsernameDoesNotContainAtSign] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);
                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.UsernameCannotContainAtSign));
                }
                return null;
            });

        static readonly char[] SpecialChars = { '.', ' ', '_', '-', '\'' };

        public static bool IsValidUsernameChar(char c)
        {
            return
                Char.IsLetterOrDigit(c) || 
                SpecialChars.Contains(c);
        }

        public static readonly IValidator<UserAccount> UsernameOnlySingleInstanceOfSpecialCharacters =
                   new DelegateValidator((service, account, value) =>
                   {
                       foreach(var specialChar in SpecialChars)
                       {
                           var doubleChar = specialChar.ToString() + specialChar.ToString();
                           if (value.Contains(doubleChar))
                           {
                               Tracing.Verbose("[UserAccountValidation.UsernameOnlySingleInstanceOfSpecialCharacters] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);
                               return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.UsernameCannotRepeatSpecialCharacters));
                           }
                       }

                       return null;
                   });
        
        public static readonly IValidator<UserAccount> UsernameOnlyContainsValidCharacters =
            new DelegateValidator((service, account, value) =>
            {
                if (!value.All(IsValidUsernameChar))
                {
                    Tracing.Verbose("[UserAccountValidation.UsernameOnlyContainsValidCharacters] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.UsernameOnlyContainsValidCharacters));
                }
                return null;
            });

        public static readonly IValidator<UserAccount> UsernameCanOnlyStartOrEndWithLetterOrDigit =
                   new DelegateValidator((service, account, value) =>
                   {
                       if (!Char.IsLetterOrDigit(value.First()) || !Char.IsLetterOrDigit(value.Last()))
                       {
                           Tracing.Verbose("[UserAccountValidation.UsernameCanOnlyStartOrEndWithLetterOrDigit] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                           return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.UsernameCanOnlyStartOrEndWithLetterOrDigit));
                       }
                       return null;
                   });
        
        public static readonly IValidator<UserAccount> UsernameMustNotAlreadyExist =
            new DelegateValidator((service, account, value) =>
            {
                if (service.UsernameExists(account.Tenant, value))
                {
                    Tracing.Verbose("[UserAccountValidation.EmailMustNotAlreadyExist] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.UsernameAlreadyInUse));
                }
                return null;
            });

        public static readonly IValidator<UserAccount> EmailRequired =
            new DelegateValidator((service, account, value) =>
            {
                if (service.Configuration.RequireAccountVerification &&
                    String.IsNullOrWhiteSpace(value))
                {
                    Tracing.Verbose("[UserAccountValidation.EmailRequired] validation failed: {0}, {1}", account.Tenant, account.Username);

                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.EmailRequired));
                }
                return null;
            });

        public static readonly IValidator<UserAccount> EmailIsValidFormat =
            new DelegateValidator((service, account, value) =>
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    EmailAddressAttribute validator = new EmailAddressAttribute();
                    if (!validator.IsValid(value))
                    {
                        Tracing.Verbose("[UserAccountValidation.EmailIsValidFormat] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                        return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.InvalidEmail));
                    }
                }
                return null;
            });

        public static readonly IValidator<UserAccount> EmailIsRequiredIfRequireAccountVerificationEnabled =
            new DelegateValidator((service, account, value) =>
            {
                if (service.Configuration.RequireAccountVerification && String.IsNullOrWhiteSpace(value))
                {
                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.EmailRequired));
                }
                return null;
            });

        public static readonly IValidator<UserAccount> EmailMustNotAlreadyExist =
            new DelegateValidator((service, account, value) =>
            {
                if (!String.IsNullOrWhiteSpace(value) && service.EmailExistsOtherThan(account, value))
                {
                    Tracing.Verbose("[UserAccountValidation.EmailMustNotAlreadyExist] validation failed: {0}, {1}, {2}", account.Tenant, account.Username, value);

                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.EmailAlreadyInUse));
                }
                return null;
            });

        public static readonly IValidator<UserAccount> PhoneNumberRequired =
            new DelegateValidator((service, account, value) =>
            {
                if (service.Configuration.RequireAccountVerification &&
                    String.IsNullOrWhiteSpace(value))
                {
                    Tracing.Verbose("[UserAccountValidation.PhoneNumberRequired] validation failed: {0}, {1}", account.Tenant, account.Username);

                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.MobilePhoneRequired));
                }
                return null;
            });

        public static readonly IValidator<UserAccount> PhoneNumberIsRequiredIfRequireAccountVerificationEnabled =
            new DelegateValidator((service, account, value) =>
            {
                if (service.Configuration.RequireAccountVerification && String.IsNullOrWhiteSpace(value))
                {
                    return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.MobilePhoneMustBeDifferent));
                }
                return null;
            });

        public static readonly IValidator<UserAccount> PasswordMustBeDifferentThanCurrent =
            new DelegateValidator((service, account, value) =>
        {
            // Use LastLogin null-check to see if it's a new account
            // we don't want to run this logic if it's a new account
            if (!account.IsNew() && service.VerifyHashedPassword(account, value))
            {
                Tracing.Verbose("[UserAccountValidation.PasswordMustBeDifferentThanCurrent] validation failed: {0}, {1}", account.Tenant, account.Username);

                return new ValidationResult(service.GetValidationMessage(UserAccountConstants.ValidationMessages.NewPasswordMustBeDifferent));
            }
            return null;
        });
    }
}
