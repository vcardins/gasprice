
namespace GasPrice.Core.ViewModels
{
    public class AppSettingsOutput : AppSettingsInput
    {
        public AppSettingsOutput()
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
