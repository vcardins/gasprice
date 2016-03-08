using System;
using System.Collections.Generic;
using GasPrice.Core.Account;
using GasPrice.Core.Common.Information;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Common.Messaging.Interfaces;
using GasPrice.Core.Common.Messaging.Models;

namespace GasPrice.Services.Messaging.Email
{
    public abstract class EmailEventHandler<TEvent> : MessagingEventHandler<TEvent>
    {
        readonly IMessagingFormatter<TEvent> _messageFormatter;
        private readonly IEmailService _emailService;

        protected EmailEventHandler(IEmailService emailService,
                                    IApplicationInformation appInfo,
                                    IMessagingFormatter<TEvent> formatter) 
            :base(appInfo)
        {
            if (emailService == null) throw new ArgumentNullException("emailService");

            _emailService = emailService;
            _messageFormatter = formatter;
        }

        protected Dictionary<string, string> GetExtraProperties(UserAccount account, object extra = null)
        {
            var data = base.GetExtraProperties(extra);
            data.Add("EmailSignature", AppInfo.EmailSignature);
            data.Add("LoginUrl", AppInfo.HomePage + AppInfo.LoginUrl); 

            if (account == null) return data;

            data.Add("DisplayName", account.DisplayName ?? "User");
            data.Add("FullName", account.FullName ?? "User");                    
            data.Add("Username", account.Username );
            data.Add("Email", account.Email);

            return data;
        }

        private EmailMessage GetEmailBody(TEvent evt, IDictionary<string, string> extra)
        {
            var body = _messageFormatter.GetMessageBody(Module, GetEvent(evt), extra);
            return body == null ? null : new EmailMessage { From = AppInfo.Email, Body = body };
        }

        private EmailSubject GetEmailSubject(TEvent evt, IDictionary<string, string> data = null)
        {
            var subject = _messageFormatter.GetMessageSubject(Module, GetEvent(evt), data);
            return new EmailSubject { Subject = subject ?? string.Empty };
        }

        protected virtual void Send(TEvent evt, string to, IDictionary<string, string> data)
        {
             var message = GetEmailBody(evt, data);

            if (message == null)
            {
                Tracing.Error("[EmailEventHandler.Send] : " + String.Format("Template for {0} missing", evt.GetType().Name));
                return;
            }

            var subject = GetEmailSubject(evt, data);

            message.Subject = subject.Subject;
            message.AsHtml = true;
            message.To = to;

            _emailService.SendAsync(message); 
        }
    }
}
