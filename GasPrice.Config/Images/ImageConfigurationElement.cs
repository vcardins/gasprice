using System.Configuration;
using GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces;

namespace GasPrice.Config.Images
{
    /// <summary>
    /// The image section.
    /// </summary>
    public class ImageConfigurationElement : ConfigurationElement, IImageSettings
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
        [ConfigurationProperty("defaultProvider", DefaultValue = "SqlProvider")]
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
        /// <summary>
        /// Gets or sets the default provider.
        /// </summary>
        [ConfigurationProperty("defaultAvatar")]
        public string DefaultAvatar
        {
            get
            {
                return (string)base["defaultAvatar"];
            }

            set
            {
                base["defaultAvatar"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the default provider.
        /// </summary>
        [ConfigurationProperty("defaultAvatarPicture")]
        public string DefaultAvatarPicture
        {
            get
            {
                return (string)base["defaultAvatarPicture"];
            }

            set
            {
                base["defaultAvatarPicture"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the default provider.
        /// </summary>
        [ConfigurationProperty("imageCacheUrl")]
        public string ImageCacheUrl
        {
            get
            {
                return (string)base["imageCacheUrl"];
            }

            set
            {
                base["imageCacheUrl"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the default provider.
        /// </summary>
        [ConfigurationProperty("tempCacheUrl")]
        public string TempCacheUrl
        {
            get
            {
                return (string)base["tempCacheUrl"];
            }

            set
            {
                base["tempCacheUrl"] = value;
            }
        }

        
        /// <summary>
        /// Gets the image resizes.
        /// </summary>
        [ConfigurationProperty("imageResize")]
        public ImageResizeCollection ImageResizes
        {
            get
            {
                return (ImageResizeCollection)base["imageResize"];
            }
        }
    }
}