/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Diagnostics;
using GasPrice.Core.EventHandling;

namespace GasPrice.Core.Account.Events
{
    public class DebuggerEventHandler : 
        IEventHandler<AccountCreatedEvent>,
        IEventHandler<PasswordResetRequestedEvent>,
        IEventHandler<EmailChangeRequestedEvent>,
        IEventHandler<EmailChangedEvent>
    {
        public void Handle(AccountCreatedEvent evt)
        {
            Debug.WriteLine("AccountCreatedEvent: " + evt.VerificationKey);
        }

        public void Handle(PasswordResetRequestedEvent evt)
        {
            Debug.WriteLine("PasswordResetRequestedEvent: " + evt.VerificationKey);
        }

        public void Handle(EmailChangeRequestedEvent evt)
        {
            Debug.WriteLine("EmailChangeRequestedEvent: " + evt.VerificationKey);
        }

        public void Handle(EmailChangedEvent evt)
        {
            Debug.WriteLine("EmailChangedEvent: " + evt.VerificationKey);
        }

    }

}
