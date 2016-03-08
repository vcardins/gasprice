#region credits
// ***********************************************************************
// Assembly	: GasPrice.Core
// Author	: Victor Cardins
// Created	: 02-24-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System.ComponentModel;

namespace GasPrice.Core.Account.Enum
{

    public enum AccountVerificationStatus
    {
        [Description("Your account was verified successfully. You can now login.")]
        Success,

        [Description("Verification code is required.")]
        MissingCode,

        [Description("Sorry, your account is closed. Contact System Administrator for more information.")]
        AccountClosed,

        [Description("Sorry, you are not allowed to login at this moment.")]
        LoginNotAllowed,

        [Description("Two factor auth mode not mobile.")]
        TwoFactorAuthNotMobile,

        [Description("Current auth status not mobile.")]
        CurrentTwoFactorAuthStatusNotMobile,

        [Description("Mobile code failed to verify.")]
        WrongVerificationCode,

        [Description("Invalid Username.")]
        InvalidUsername,

        [Description("Invalid Phone number.")]
        InvalidPhoneNumber,

        [Description("Invalid Verification key.")]
        InvalidVerificationKey
        
    }
}