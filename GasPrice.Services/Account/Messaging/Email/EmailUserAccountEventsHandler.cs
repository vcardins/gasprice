using GasPrice.Core.Account.Events;
using GasPrice.Core.Common.Information;
using GasPrice.Core.Common.Messaging.Interfaces;
using GasPrice.Core.EventHandling;

namespace GasPrice.Services.Account.Messaging.Email
{
    public class EmailUserAccountEventsHandler :
        EmailUserAccountEventHandler,
        IEventHandler<AccountCreatedEvent>,        
        IEventHandler<PasswordResetRequestedEvent>,
        IEventHandler<PasswordChangedEvent>,
        IEventHandler<UsernameReminderRequestedEvent>,
        IEventHandler<AccountClosedEvent>,
        IEventHandler<AccountReopenedEvent>,
        IEventHandler<UsernameChangedEvent>,
        IEventHandler<EmailChangeRequestedEvent>,
        IEventHandler<EmailChangedEvent>,
        IEventHandler<EmailVerifiedEvent>
    {

        public EmailUserAccountEventsHandler(IEmailService emailService,
                                             IApplicationInformation appInfo,
                                             IMessagingFormatter<UserAccountEvent> formatter)
            : base(emailService, appInfo, formatter)
        {
        }

        public void Handle(AccountCreatedEvent evt)
        {
            Process(evt, new { evt.InitialPassword, evt.VerificationKey });
        }
        
        public void Handle(PasswordResetRequestedEvent evt)
        {
            Process(evt, new { evt.VerificationKey });
        }

        public void Handle(PasswordChangedEvent evt)
        {
            Process(evt, new { evt.NewPassword });
        }

        public void Handle(UsernameReminderRequestedEvent evt)
        {
            Process(evt);
        }

        public void Handle(AccountClosedEvent evt)
        {
            Process(evt);
        }
        
        public void Handle(AccountReopenedEvent evt)
        {
            Process(evt, new { evt.VerificationKey });
        }

        public void Handle(UsernameChangedEvent evt)
        {
            Process(evt);
        }

        public void Handle(EmailChangeRequestedEvent evt)
        {
            Process(evt, new{evt.OldEmail, evt.NewEmail, evt.VerificationKey});
        }

        public void Handle(EmailChangedEvent evt)
        {
            Process(evt, new { evt.OldEmail, evt.VerificationKey });
        }
        
        public void Handle(EmailVerifiedEvent evt)
        {
            Process(evt);
        }

    }
}