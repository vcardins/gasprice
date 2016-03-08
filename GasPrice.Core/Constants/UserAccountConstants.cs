using System;

namespace GasPrice.Core.Constants
{
    public class UserAccountConstants
    {

        public class ClaimTypes
        {
            public const string Tenant = "http://brockallen.com/membershipreboot/claims/tenant";
        }

        public class SecuritySettingDefaults
        {
            public const bool MultiTenant = false;
            public const string DefaultTenant = "default";
            public const bool EmailIsUsername = false;
            public const bool UsernamesUniqueAcrossTenants = false;
            public const bool RequireAccountVerification = true;
            public const bool AllowLoginAfterAccountCreation = true;
            public const int AccountLockoutFailedLoginAttempts = 5;
            public const string AccountLockoutDuration = "00:05:00";
            public const bool AllowAccountDeletion = true;
            public const int PasswordHashingIterationCount = 0;
            public const int PasswordResetFrequency = 0;
        }

        public class InformationMessages
        {
            public const string AccessNotAllowed = "Você não está autorizado a realizar essa ação";
            public const string InvalidRequestParameters = "Parametros inválidos";
            public const string InvalidNameIdentifier = "Identificado Inválido";
            public const string NotFound = "{0} não foi localizado";
            public const string NotFound2 = "{0} ({1}) não foi localizado";
        }

        public class UserAccount
        {
            public const int VerificationKeyStaleDurationMinutes = 20;
            public const int MobileCodeLength = 4;
            public const int SecurityPinLength = 4;
            public const int MobileCodeResendDelayMinutes = 1;
            public const int MobileCodeStaleDurationMinutes = 2880;
            public const int TwoFactorAuthTokenDurationDays = 30;
        }

        public class AuthService
        {
            public static readonly TimeSpan TwoFactorAuthTokenLifetime = TimeSpan.FromMinutes(10);
            public const int DefaultPersistentCookieDays = UserAccount.TwoFactorAuthTokenDurationDays;
            public const string CookieBasedTwoFactorAuthPolicyCookieName = "mr.cbtfap.";
        }

        public class PasswordComplexity
        {
            public const int MinimumLength = 10;
            public const int NumberOfComplexityRules = 3;
        }

        public static class ValidationMessages
        {
            public const string AccountAlreadyVerified = "AccountAlreadyVerified";
            public const string AccountCreateFailNoEmailFromIdp = "AccountCreateFailNoEmailFromIdp";
            public const string AccountNotConfiguredWithSecretQuestion = "AccountNotConfiguredWithSecretQuestion";
            public const string AccountNotVerified = "AccountNotVerified";
            public const string AccountPasswordResetRequiresSecretQuestion = "AccountPasswordResetRequiresSecretQuestion";
            public const string AddClientCertForTwoFactor = "AddClientCertForTwoFactor";
            public const string CantRemoveLastLinkedAccountIfNoPassword = "CantRemoveLastLinkedAccountIfNoPassword";
            public const string CertificateAlreadyInUse = "CertificateAlreadyInUse";
            public const string CodeRequired = "CodeRequired";
            public const string EmailAlreadyInUse = "EmailAlreadyInUse";
            public const string PhoneNumberAlreadyInUse = "PhoneNumberAlreadyInUse";
            public const string EmailRequired = "EmailRequired";
            public const string InvalidCertificate = "InvalidCertificate";
            public const string InvalidEmail = "InvalidEmail";
            public const string InvalidKey = "InvalidKey";
            public const string InvalidName = "InvalidName";
            public const string InvalidNewPassword = "InvalidNewPassword";
            public const string InvalidOldPassword = "InvalidOldPassword";
            public const string InvalidPassword = "InvalidPassword";
            public const string InvalidNewSecurityPin = "InvalidNewSecurityPin";
            public const string InvalidOldSecurityPin = "InvalidOldSecurityPin";
            public const string InvalidSecurityPin = "InvalidSecurityPin";
            public const string InvalidPhone = "InvalidPhone";
            public const string InvalidQuestionOrAnswer = "InvalidQuestionOrAnswer";
            public const string InvalidTenant = "InvalidTenant";
            public const string InvalidUsername = "InvalidUsername";
            public const string LoginFailEmailAlreadyAssociated = "LoginFailEmailAlreadyAssociated";
            public const string LoginNotAllowed = "LoginNotAllowed";
            public const string MobilePhoneAlreadyInUse = "MobilePhoneAlreadyInUse";
            public const string MobilePhoneMustBeDifferent = "MobilePhoneMustBeDifferent";
            public const string MobilePhoneRequired = "MobilePhoneRequired";
            public const string NameAlreadyInUse = "NameAlreadyInUse";
            public const string NameRequired = "NameRequired";
            public const string NewPasswordMustBeDifferent = "NewPasswordMustBeDifferent";
            public const string NewSecurityPinMustBeAtLeastFourDigits = "NewSecurityPinMustBeAtLeastFourDigits";
            public const string ParentGroupAlreadyChild = "ParentGroupAlreadyChild";
            public const string ParentGroupSameAsChild = "ParentGroupSameAsChild";
            public const string PasswordComplexityRules = "PasswordComplexityRules";
            public const string PasswordLength = "PasswordLength";
            public const string PasswordRequired = "PasswordRequired";
            public const string PasswordResetErrorNoEmail = "PasswordResetErrorNoEmail";
            public const string SecurityPinResetErrorNoPhone = "SecurityPinResetErrorNoPhone";
            public const string RegisterMobileForTwoFactor = "RegisterMobileForTwoFactor";
            public const string ReopenErrorNoEmail = "ReopenErrorNoEmail";
            public const string SecretAnswerRequired = "SecretAnswerRequired";
            public const string SecretQuestionAlreadyInUse = "SecretQuestionAlreadyInUse";
            public const string SecretQuestionRequired = "SecretQuestionRequired";
            public const string TenantRequired = "TenantRequired";
            public const string UsernameAlreadyInUse = "UsernameAlreadyInUse";
            public const string UsernameCannotContainAtSign = "UsernameCannotContainAtSign";
            public const string UsernameOnlyContainLettersAndDigits = "UsernameOnlyContainLettersAndDigits";
            public const string UsernameRequired = "UsernameRequired";

            public const string UsernameOnlyContainsValidCharacters = "UsernameOnlyContainsValidCharacters";
            public const string UsernameCannotRepeatSpecialCharacters = "UsernameCannotRepeatSpecialCharacters";
            public const string UsernameCanOnlyStartOrEndWithLetterOrDigit = "UsernameCanOnlyStartOrEndWithLetterOrDigit";
        }
    }
}
