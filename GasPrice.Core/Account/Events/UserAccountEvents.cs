/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */


using System.Security.Cryptography.X509Certificates;
using GasPrice.Core.EventHandling;

namespace GasPrice.Core.Account.Events
{
    interface IAllowMultiple { }

    public abstract class UserAccountEvent : IEvent
    {
        public UserAccount Account { get; set; }
    }

    public class AccountCreatedEvent : UserAccountEvent
    {
        // InitialPassword might be null if this is a re-send
        // notification for account created (when user tries to
        // reset password before verifying their account)
        public string InitialPassword { get; set; }
        public string VerificationKey { get; set; }
    }

    public class PasswordResetFailedEvent : UserAccountEvent { }
    public class PasswordResetRequestedEvent : UserAccountEvent
    {
        public string VerificationKey { get; set; }
    }
    public class PasswordChangedEvent : UserAccountEvent
    {
        public string NewPassword { get; set; }
    }
  
    public class UsernameReminderRequestedEvent : UserAccountEvent { }
    public class AccountClosedEvent : UserAccountEvent { }
    public class AccountReopenedEvent : UserAccountEvent
    {
        public string VerificationKey { get; set; }
    }
    public class UsernameChangedEvent : UserAccountEvent { }
    
    public class EmailChangeRequestedEvent : UserAccountEvent
    {
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
        public string VerificationKey { get; set; }
    }
    public class EmailChangedEvent : UserAccountEvent
    {
        public string OldEmail { get; set; }
        public string VerificationKey { get; set; }
    }
    public class EmailVerifiedEvent : UserAccountEvent { }

    public class ClaimAddedEvent : UserAccountEvent, IAllowMultiple
    {
        public UserClaim Claim { get; set; }
    }
    public class ClaimRemovedEvent : UserAccountEvent, IAllowMultiple
    {
        public UserClaim Claim { get; set; }
    }

    public abstract class SuccessfulLoginEvent : UserAccountEvent { }
    public class SuccessfulPasswordLoginEvent : SuccessfulLoginEvent { }
   
    public abstract class FailedLoginEvent : UserAccountEvent { }
    public class AccountNotVerifiedEvent : FailedLoginEvent { }
    public class AccountLockedEvent : FailedLoginEvent { }
    public class InvalidAccountEvent : FailedLoginEvent { }
    public class TooManyRecentPasswordFailuresEvent : FailedLoginEvent { }
    public class InvalidPasswordEvent : FailedLoginEvent { }
    public class InvalidCertificateEvent : FailedLoginEvent
    {
        public X509Certificate2 Certificate { get; set; }
    }
}
