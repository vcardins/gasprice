using System;
using System.Configuration;
using GasPrice.Core.Config.Security;

namespace GasPrice.Config.Security
{
    public class SecurityElement : ConfigurationElement, ISecuritySettings
    {
        [ConfigurationProperty("multiTenant", DefaultValue = false)]
        public bool MultiTenant
        {
            get { return (bool)this["multiTenant"]; }
            set { this["multiTenant"] = value; }
        }

        [ConfigurationProperty("defaultTenant", DefaultValue = "default")]
        public string DefaultTenant
        {
            get { return (string)this["defaultTenant"]; }
            set { this["defaultTenant"] = value; }
        }

        [ConfigurationProperty("emailIsUsername", DefaultValue = false)]
        public bool EmailIsUsername
        {
            get { return (bool)this["emailIsUsername"]; }
            set { this["emailIsUsername"] = value; }
        }

        [ConfigurationProperty("usernamesUniqueAcrossTenants", DefaultValue = false)]
        public bool UsernamesUniqueAcrossTenants
        {
            get { return (bool)this["usernamesUniqueAcrossTenants"]; }
            set { this["usernamesUniqueAcrossTenants"] = value; }
        }

        [ConfigurationProperty("requireAccountVerification", DefaultValue = true)]
        public bool RequireAccountVerification
        {
            get { return (bool)this["requireAccountVerification"]; }
            set { this["requireAccountVerification"] = value; }
        }

        [ConfigurationProperty("allowLoginAfterAccountCreation", DefaultValue = true)]
        public bool AllowLoginAfterAccountCreation
        {
            get { return (bool)this["allowLoginAfterAccountCreation"]; }
            set { this["allowLoginAfterAccountCreation"] = value; }
        }

        [ConfigurationProperty("accountLockoutFailedLoginAttempts", DefaultValue = 10)]
        public int AccountLockoutFailedLoginAttempts
        {
            get { return (int)this["accountLockoutFailedLoginAttempts"]; }
            set { this["accountLockoutFailedLoginAttempts"] = value; }
        }

        [ConfigurationProperty("accountLockoutDuration", DefaultValue = "00:05:00")]
        public TimeSpan AccountLockoutDuration
        {
            get { return (TimeSpan)this["accountLockoutDuration"]; }
            set { this["accountLockoutDuration"] = value; }
        }

        [ConfigurationProperty("allowAccountDeletion", DefaultValue = true)]
        public bool AllowAccountDeletion
        {
            get { return (bool)this["allowAccountDeletion"]; }
            set { this["allowAccountDeletion"] = value; }
        }

        [ConfigurationProperty("minPasswordLength", DefaultValue = 4)]
        public int MinimumPasswordLength
        {
            get { return (int)this["minPasswordLength"]; }
            set { this["minPasswordLength"] = value; }
        }

        [ConfigurationProperty("passwordResetFrequency", DefaultValue = 0)]
        public int PasswordResetFrequency
        {
            get { return (int)this["passwordResetFrequency"]; }
            set { this["passwordResetFrequency"] = value; }
        }

        [ConfigurationProperty("passwordHashingIterationCount", DefaultValue = 0)]
        public int PasswordHashingIterationCount
        {
            get { return (int)this["passwordHashingIterationCount"]; }
            set { this["passwordHashingIterationCount"] = value; }
        }

        [ConfigurationProperty("allowEmailChangeWhenEmailIsUsername", DefaultValue = false)]
        public bool AllowEmailChangeWhenEmailIsUsername
        {
            get { return (bool)this["allowEmailChangeWhenEmailIsUsername"]; }
            set { this["allowEmailChangeWhenEmailIsUsername"] = value; }
        }

        [ConfigurationProperty("allowMultipleMobileLogin", DefaultValue = false)]
        public bool AllowMultipleMobileLogin
        {
            get { return (bool)this["allowMultipleMobileLogin"]; }
            set { this["allowMultipleMobileLogin"] = value; }
        }

        [ConfigurationProperty("verificationKeyLifetime", DefaultValue = "00:15:00")]
        public TimeSpan VerificationKeyLifetime
        {
            get { return (TimeSpan)this["verificationKeyLifetime"]; }
            set { this["verificationKeyLifetime"] = value; }
        }
    }
}
