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
    public enum UserAccountActionMessages
    {
        [Description("You'll soon receive a SMS with instructions to verify your phone number and activate your account")]
        SuccessOnCreatingSmsValidation,

        [Description("You'll soon receive an email with instructions to verify and activate your account")]
        SuccessOnCreatingEmailValidation,

        [Description("Your account has been successfully updated.")]
        SuccessOnUpdating,

        [Description("Your account has been successfully verified.")]
        SuccessOnVerifying,

        [Description("Your account has been successfully canceled.")]
        SuccessOnCanceling,

        [Description("Your account has been successfully deleted.")]
        SuccessOnClosing,

        [Description("Your password has been successfully updated.")]
        SuccessOnUpdatingPassword,

        [Description("Username is invalid.")]
        InvalidUsername,

        [Description("Password is invalid.")]
        InvalidPassword,

        [Description("SecurityPin is invalid.")]
        InvalidSecurityPin,

        [Description("Email is invalid.")]
        InvalidEmail,

        [Description("Phone is invalid.")]
        InvalidPhone,

        [Description("Missing Tenant.")]
        MissingTenant,

        [Description("Password is required.")]
        MissingPassword,

        [Description("Username is required.")]
        MissingUsername,

        [Description("Your account is not activated yet.")]
        AccountNotActivated,

        [Description("Email already in use")]
        EmailAlreadyInUse,

        [Description("Phone number already in use")]
        PhoneAlreadyInUse,

        [Description("Your customer account could not be .")]
        ErrorOnCreatingCustomerAccount,

        [Description("User not found")]
        UserNotFound,

        [Description("You should recieve an email shortly with your username.")]
        UsernameReminderSent
        
    }
}