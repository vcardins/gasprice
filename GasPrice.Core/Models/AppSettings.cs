
namespace GasPrice.Core.Models
{
    public class AppSettings : BaseObjectState
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string Keywords { get; set; }
        public string Version { get; set; }
        public string Email { get; set; }
        public string EmailSignature { get; set; }
        public string LongDateTimeFormat { get; set; }
        public string ShortDateTimeFormat { get; set; }
        public string TinyDateTimeFormat { get; set; }
        public int CacheDurationInMinutes { get; set; }
        public string GoogleAnalyticsKey { get; set; }

        //Security
        public bool RequireAccountVerification { get; set; }
        public bool EmailIsUsername { get; set; }
        public bool MultiTenant { get; set; }
        public bool AllowAccountDeletion { get; set; }
        public int PasswordHashingIterationCount { get; set; }
        public int AccountLockoutFailedLoginAttempts { get; set; }
        public int AccountLockoutDuration { get; set; }
        public int PasswordResetFrequency { get; set; }
        public bool Active { get; set; }

        public AppSettings()
        {
            CacheDurationInMinutes = 120;
            RequireAccountVerification = true;
            EmailIsUsername = false;
            MultiTenant = false;
            AllowAccountDeletion = false;
            PasswordHashingIterationCount = 10;
            AccountLockoutFailedLoginAttempts = 20;
            AccountLockoutDuration = 10;
            PasswordResetFrequency = 10000;
            Active = true;
        }
     
    }
}
