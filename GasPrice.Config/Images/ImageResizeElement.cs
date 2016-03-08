using System.Configuration;
using GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces;

namespace GasPrice.Config.Images
{
    /// <summary>
    /// The photo resize element.
    /// </summary>
    public class ImageResizeElement : ConfigurationElement, IImageResize
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }

            set
            {
                base["name"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [ConfigurationProperty("width")]
        public int Width
        {
            get
            {
                return (int)base["width"];
            }

            set
            {
                base["width"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [ConfigurationProperty("height")]
        public int Height
        {
            get
            {
                return (int)base["height"];
            }

            set
            {
                base["height"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get
            {
                return (bool)base["enabled"];
            }

            set
            {
                base["enabled"] = value;
            }
        }
    }
}