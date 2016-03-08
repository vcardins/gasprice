using System.Configuration;
using GasPrice.Core.Config.Messaging;

namespace GasPrice.Config.Messaging
{

    /// <summary>
    /// Base class for push notification settings
    /// </summary>
    public abstract class BaseMessagingServiceElement : ConfigurationElement, IMessagingServiceSettings
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [ConfigurationProperty("title", IsRequired = false)]
        public string Title
        {
            get { return (string)base["title"]; }
            set { base["title"] = value; }
        }

    }
}