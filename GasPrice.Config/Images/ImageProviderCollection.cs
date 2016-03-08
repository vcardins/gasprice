using System;
using System.Configuration.Provider;
using GasPrice.Core.Common.Infrastructure.ImageHandler;

namespace GasPrice.Config.Images
{
    /// <summary>
    /// The photo provider collection.
    /// </summary>
    public class ImageProviderCollection : ProviderCollection
    {
        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The JamesRocks.Images.ImageProvider.
        /// </returns>
        public new IImageProvider this[string name]
        {
            get
            {
                return (IImageProvider)base[name];
            }
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (!(provider is IImageProvider))
            {
                throw new ArgumentException("Invalid provider type", "provider");
            }

            base.Add(provider);
        }
    }
}