using GasPrice.Core.Account.Events;
using GasPrice.Core.Common.Information;
using GasPrice.Core.Common.Messaging.Interfaces;
using GasPrice.Services.Messaging.Email;

namespace GasPrice.Services.Account.Messaging.Email
{
    public class EmailUserAccountEventHandler : EmailEventHandler<UserAccountEvent>
    {

        public EmailUserAccountEventHandler(IEmailService emailService,
                                            IApplicationInformation appInfo,
                                            IMessagingFormatter<UserAccountEvent> formatter)
            : base(emailService, appInfo, formatter)
        {

        }

        public virtual void Process(AccountCreatedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            data.Add("ConfirmChangeEmailUrl", AppInfo.AppUrl + AppInfo.ConfirmChangeEmailUrl + data["VerificationKey"]);
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(AccountClosedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(EmailVerifiedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(PasswordChangedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);          
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(PasswordResetRequestedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            data.Add("ConfirmPasswordResetUrl", AppInfo.AppUrl + AppInfo.ConfirmPasswordResetUrl + data["VerificationKey"]);
            data.Add("CancelVerificationUrl", AppInfo.AppUrl + AppInfo.CancelVerificationUrl + data["VerificationKey"]);
            
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(UsernameReminderRequestedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(UsernameChangedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(EmailChangedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            data.Add("ConfirmChangeEmailUrl", AppInfo.AppUrl + AppInfo.ConfirmChangeEmailUrl + data["VerificationKey"]);
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(EmailChangeRequestedEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            data.Add("ConfirmChangeEmailUrl", AppInfo.AppUrl + AppInfo.ConfirmChangeEmailUrl + data["VerificationKey"]);
            Send(evt, evt.Account.Email, data);
        }

        public virtual void Process(UserAccountEvent evt, object extra = null)
        {
            var data = GetExtraProperties(evt.Account, extra);
            Send(evt, evt.Account.Email, data);
        }

    }
}
