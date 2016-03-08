
namespace GasPrice.Core.ViewModels
{
    public class AppSettingsInput
    {        
        //[Required]
        public string ShortName { get; set; }
        //[Required]
        public string LongName { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string Keywords { get; set; }
        public string Version { get; set; }
        //[Required]
        public string Email { get; set; }
        public string EmailSignature { get; set; }
        //[Required]
        public string LongDateTimeFormat { get; set; }
        //[Required]
        public string ShortDateTimeFormat { get; set; }
        //[Required]
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
     
    }
}
