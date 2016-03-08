using System;
using System.Configuration;
using GasPrice.Core.Config.Messaging;

namespace GasPrice.Config.Messaging
{
    /// <summary>
    /// The photo section.
    /// </summary>
    public class SendGridMailerConfigurationElement : ConfigurationElement
    {

        /// <summary>
        /// The lock.
        /// </summary>
        private static readonly object Lock = new object();

        /// <summary>
        /// Gets the web services.
        /// </summary>
        /// <value>
        /// The web services.
        /// </value>
        [ConfigurationProperty("environments")]
        public SendGridMailerCollection Environments
        {
            get { return (SendGridMailerCollection)base["environments"]; }

        }

        /// <value>
        /// The base URL.
        /// </value>
        [ConfigurationProperty("default")]
        public string Default
        {
            get { return (string)base["default"]; }

            set { base["default"] = value; }
        }

        /// <value>
        /// The base URL.
        /// </value>
        public ISendGridMailerSettings Current
        {
            get
            {
                lock (Lock)
                {
                    if (Environments[Default] == null)
                    {
                        throw new Exception("Unable to load default context");
                    }
                }
                return Environments[Default];
            }

        }

    }
}