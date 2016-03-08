/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using GasPrice.Core.Account.Validation;
using GasPrice.Core.Config.Security;
using GasPrice.Core.EventHandling;

namespace GasPrice.Core.Account.Configuration
{
    public class MembershipRebootConfiguration
    {
        public MembershipRebootConfiguration(ISecuritySettings securitySettings)
        {
            if (securitySettings == null) throw new ArgumentNullException("securitySettings");
            
            MultiTenant = securitySettings.MultiTenant;
            DefaultTenant = securitySettings.DefaultTenant;
            EmailIsUsername = securitySettings.EmailIsUsername;
            UsernamesUniqueAcrossTenants = securitySettings.UsernamesUniqueAcrossTenants;
            RequireAccountVerification = securitySettings.RequireAccountVerification;
            AllowLoginAfterAccountCreation = securitySettings.AllowLoginAfterAccountCreation;
            AccountLockoutFailedLoginAttempts = securitySettings.AccountLockoutFailedLoginAttempts;
            AccountLockoutDuration = securitySettings.AccountLockoutDuration;
            AllowAccountDeletion = securitySettings.AllowAccountDeletion;
            PasswordHashingIterationCount = securitySettings.PasswordHashingIterationCount;
            PasswordResetFrequency = securitySettings.PasswordResetFrequency;
            VerificationKeyLifetime = securitySettings.VerificationKeyLifetime;

        }

        public bool MultiTenant { get; set; }
        public string DefaultTenant { get; set; }
        public bool EmailIsUsername { get; set; }
        public bool UsernamesUniqueAcrossTenants { get; set; }
        public bool RequireAccountVerification { get; set; }
        public bool AllowLoginAfterAccountCreation { get; set; }
        public int AccountLockoutFailedLoginAttempts { get; set; }
        public TimeSpan AccountLockoutDuration { get; set; }
        public bool AllowAccountDeletion { get; set; }
        public int PasswordHashingIterationCount { get; set; }
        public int PasswordResetFrequency { get; set; }
        public TimeSpan VerificationKeyLifetime { get; set; }

        AggregateValidator<UserAccount> usernameValidators = new AggregateValidator<UserAccount>();
        public void RegisterUsernameValidator(params IValidator<UserAccount>[] items)
        {
            usernameValidators.AddRange(items);
        }
        public IValidator<UserAccount> UsernameValidator { get { return usernameValidators; } }

        AggregateValidator<UserAccount> passwordValidators = new AggregateValidator<UserAccount>();
        public void RegisterPasswordValidator(params IValidator<UserAccount>[] items)
        {
            passwordValidators.AddRange(items);
        }
        public IValidator<UserAccount> PasswordValidator { get { return passwordValidators; } }

        AggregateValidator<UserAccount> emailValidators = new AggregateValidator<UserAccount>();
        public void RegisterEmailValidator(params IValidator<UserAccount>[] items)
        {
            emailValidators.AddRange(items);
        }
        public IValidator<UserAccount> EmailValidator { get { return phoneNumberValidators; } }

        AggregateValidator<UserAccount> phoneNumberValidators = new AggregateValidator<UserAccount>();
        public void RegisterPhoneNumberValidator(params IValidator<UserAccount>[] items)
        {
            phoneNumberValidators.AddRange(items);
        }
        public IValidator<UserAccount> PhoneNumberValidator { get { return phoneNumberValidators; } }

        EventBus eventBus = new EventBus();
        public IEventBus EventBus { get { return eventBus; } }
        public void AddEventHandler(params IEventHandler[] handlers)
        {
            foreach (var h in handlers) VerifyHandler(h);
            eventBus.AddRange(handlers);
        }
        
        EventBus validationBus = new EventBus();
        public IEventBus ValidationBus { get { return validationBus; } }
        public void AddValidationHandler(params IEventHandler[] handlers)
        {
            foreach (var h in handlers) VerifyHandler(h);
            validationBus.AddRange(handlers);
        }

        CommandBus commandBus = new CommandBus();
        public ICommandBus CommandBus { get { return commandBus; } }
        public void AddCommandHandler(ICommandHandler handler)
        {
            VerifyHandler(handler);
            commandBus.Add(handler);
        }

        private void VerifyHandler(IEventHandler e)
        {
            var type = e.GetType();
            var interfaces = type.GetInterfaces();
            foreach (var itf in interfaces)
            {
                if (itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                {
                    var eventHandlerType = itf.GetGenericArguments()[0];
                    if (eventHandlerType.IsGenericType)
                    {
                        var targetUserAccountType = eventHandlerType.GetGenericArguments()[0];
                        var isSameType = targetUserAccountType == typeof(UserAccount);
                        if (!isSameType)
                        {
                            throw new ArgumentException(String.Format("Event handler: {0} must handle events for User Account type: {1}",
                                e.GetType().FullName,
                                typeof(UserAccount).FullName));
                        }
                    }
                }
            }
        }
        private void VerifyHandler(ICommandHandler e)
        {
            var type = e.GetType();
            var interfaces = type.GetInterfaces();
            foreach (var itf in interfaces)
            {
                if (itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                {
                    var eventHandlerType = itf.GetGenericArguments()[0];
                    if (eventHandlerType.IsGenericType)
                    {
                        var targetUserAccountType = eventHandlerType.GetGenericArguments()[0];
                        var isSameType = targetUserAccountType == typeof(UserAccount);
                        if (!isSameType)
                        {
                            throw new ArgumentException(String.Format("Command handler: {0} must handle commands for User Account type: {1}",
                                e.GetType().FullName,
                                typeof(UserAccount).FullName));
                        }
                    }
                }
            }
        }
        
   
    }
    
}
