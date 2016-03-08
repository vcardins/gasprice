/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using GasPrice.Core.Account;
using GasPrice.Core.Account.Configuration;
using GasPrice.Core.Account.Validation;
using GasPrice.Core.EventHandling;

namespace GasPrice.Services.Account.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigurePasswordComplexity(this MembershipRebootConfiguration config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.RegisterPasswordValidator(new PasswordComplexityValidator());
        }

        public static void ConfigurePasswordComplexity(this MembershipRebootConfiguration config, int minimumLength, int minimumNumberOfComplexityRules)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.RegisterPasswordValidator(new PasswordComplexityValidator(minimumLength, minimumNumberOfComplexityRules));
        }

        public static void AddCommandHandler<TCommand>(this MembershipRebootConfiguration config, Action<TCommand> action)
            
            where TCommand : ICommand
        {
            if (config == null) throw new ArgumentNullException("config");
            config.AddCommandHandler(new DelegateCommandHandler<TCommand>(action));
        }

        public static void AddValidationHandler<TEvent>(this MembershipRebootConfiguration config, Action<TEvent> action)
            
            where TEvent : IEvent
        {
            if (config == null) throw new ArgumentNullException("config");
            config.AddValidationHandler(new DelegateEventHandler<TEvent>(action));
        }

        public static void AddEventHandler<TEvent>(this MembershipRebootConfiguration config, Action<TEvent> action)
            
            where TEvent : IEvent
        {
            if (config == null) throw new ArgumentNullException("config");
            config.AddEventHandler(new DelegateEventHandler<TEvent>(action));
        }

        public static void RegisterUsernameValidator(this MembershipRebootConfiguration config, Func<IUserAccountService, UserAccount, string, ValidationResult> func)
            
        {
            if (config == null) throw new ArgumentNullException("config");
            config.RegisterUsernameValidator(new DelegateValidator(func));
        }
        public static void RegisterPasswordValidator(this MembershipRebootConfiguration config, Func<IUserAccountService, UserAccount, string, ValidationResult> func)
            
        {
            if (config == null) throw new ArgumentNullException("config");
            config.RegisterPasswordValidator(new DelegateValidator(func));
        }
        public static void RegisterEmailValidator(this MembershipRebootConfiguration config, Func<IUserAccountService, UserAccount, string, ValidationResult> func)
            
        {
            if (config == null) throw new ArgumentNullException("config");
            config.RegisterEmailValidator(new DelegateValidator(func));
        }
    }
}
