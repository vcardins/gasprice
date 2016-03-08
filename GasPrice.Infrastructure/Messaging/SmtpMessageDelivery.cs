using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using GasPrice.Core.Common.Messaging.Interfaces;
using GasPrice.Core.Common.Messaging.Models;

namespace GasPrice.Infrastructure.Messaging
{

    public class SmtpMessageDelivery : IEmailService
    {

        private static readonly SmtpSection Config;
        private static SmtpClient _smtp;

        static SmtpMessageDelivery()
        {
            Config = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            
        }

        public SmtpMessageDelivery()
        {
            _smtp = new SmtpClient();
        }

        public Task SendAsync(EmailMessage message, EmailAttachment attachment = null)
        {
            var mailMessage = GetMessage(message, attachment);
            _smtp.SendAsync(mailMessage,  null);
            return null;
        }

        public MailMessage GetMessage(EmailMessage message, EmailAttachment attachment = null) {

            if (String.IsNullOrWhiteSpace(message.From))
            {
                message.From = Config.From;
            }
           
            var mailMessage = new MailMessage
            {
                From = new MailAddress(message.From), 
                IsBodyHtml = message.AsHtml, 
                Subject = message.Subject, 
                Body = message.Body,
                To = { message.Recipients == null ? message.To : message.Recipients[0] }
            };
            return mailMessage;
          
        }

       
    }
}
