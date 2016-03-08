using System.Configuration;
using GasPrice.Config.Messaging;

namespace GasPrice.Config
{
    public partial class AppConfig
    {
        [ConfigurationProperty("messaging", IsRequired = true)]
        public MessagingConfigurationElement Messaging
        {
            get
            {
                return (MessagingConfigurationElement)base["messaging"];
            }
        }
    }
}
