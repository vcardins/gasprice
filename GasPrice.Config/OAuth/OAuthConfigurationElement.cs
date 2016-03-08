using System;
using System.Configuration;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Config.OAuth;

namespace GasPrice.Config.OAuth
{
    /// <summary>
    /// The photo section.
    /// </summary>
    public class OAuthConfigurationElement : ConfigurationElement, IOAuthSettings
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
        [ConfigurationProperty("providers")]
        public OAuthProviderCollection Providers
        {
            get { return (OAuthProviderCollection)base["providers"]; }

        }

        /// <value>
        /// The base URL.
        /// </value>
        public IOAuthProviderSettings this[OAuthProvider name]
        {
            get
            {
                lock (Lock)
                {
                    if (Providers[name] == null)
                    {
                        throw new Exception("Unable to load default context");
                    }
                }
                return Providers[name];
            }

        }
    }
}