using System.Configuration;

namespace GasPrice.Config.Messaging
{
    /// <summary>
    /// The photo section.
    /// </summary>
    public class MessagingConfigurationElement : ConfigurationElement
    {

        /// <summary>
        /// Gets the Twilio push notification service settings.
        /// </summary>
        /// <value>
        /// The web services.
        /// </value>
        [ConfigurationProperty("sendgrid")]
        public SendGridMailerConfigurationElement Mailer
        {
            get { return (SendGridMailerConfigurationElement)base["sendgrid"]; }

        }
    }
}