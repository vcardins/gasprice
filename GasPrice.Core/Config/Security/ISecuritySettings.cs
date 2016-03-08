#region credits
// ***********************************************************************
// Assembly	: GasPrice.Core
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System;

namespace GasPrice.Core.Config.Security
{
    #region

    

    #endregion

    public interface ISecuritySettings
    {
        bool MultiTenant { get; set; }
        string DefaultTenant { get; set; }
        bool EmailIsUsername { get; set; }
        bool UsernamesUniqueAcrossTenants { get; set; }
        bool RequireAccountVerification { get; set; }
        bool AllowLoginAfterAccountCreation { get; set; }
        int AccountLockoutFailedLoginAttempts { get; set; }
        TimeSpan AccountLockoutDuration { get; set; }
        bool AllowAccountDeletion { get; set; }
        int MinimumPasswordLength { get; set; }
        int PasswordResetFrequency { get; set; }
        int PasswordHashingIterationCount { get; set; }
        bool AllowEmailChangeWhenEmailIsUsername { get; set; }
        bool AllowMultipleMobileLogin { get; set; }
        TimeSpan VerificationKeyLifetime { get; set; }
    }
}