using System.Configuration;

namespace GasPrice.Config.Storage
{
    public class StorageElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the providers.
        /// </summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return (ProviderSettingsCollection)base["providers"];
            }
        }

        /// <summary>
        /// Gets or sets the default provider.
        /// </summary>
        [StringValidator(MinLength = 1)]
        [ConfigurationProperty("defaultProvider", DefaultValue = "FileSystem")]
        public string DefaultProvider
        {
            get
            {
                return (string)base["defaultProvider"];
            }

            set
            {
                base["defaultProvider"] = value;
            }
        }

    }
}
