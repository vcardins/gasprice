using System.Configuration;
using GasPrice.Core.Config.Messaging;

namespace GasPrice.Config.Messaging
{

    public class SendGridMailerElement : BaseMessagingServiceElement, ISendGridMailerSettings
    {

        [ConfigurationProperty("smtpServer", IsRequired = true)]
        public string SmtpServer
        {
            get { return (string)base["smtpServer"]; }
            set { base["smtpServer"] = value; }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get { return (string)base["username"]; }
            set { base["username"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }

        [ConfigurationProperty("fromEmail", IsRequired = false)]
        public string FromEmail
        {
            get { return (string)base["fromEmail"]; }
            set { base["fromEmail"] = value; }
        }


        [ConfigurationProperty("fromName", IsRequired = false)]
        public string FromName
        {
            get { return (string)base["fromName"]; }
            set { base["fromName"] = value; }
        }

    }
}