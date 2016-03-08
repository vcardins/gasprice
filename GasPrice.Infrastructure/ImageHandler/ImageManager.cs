#region credits
// ***********************************************************************
// Assembly	: GasPrice.Infrastructure
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Configuration;
using GasPrice.Config;
using GasPrice.Config.Images;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Common.Infrastructure.ImageHandler;

namespace GasPrice.Infrastructure.ImageHandler
{
    #region

    

    #endregion

    /// <summary>
    /// The photo manager.
    /// </summary>
    public class ImageManager
    {
        #region Provider-specific code

        /// <summary>
        /// The lock.
        /// </summary>
        private static readonly object Lock = new object();

        /// <summary>
        /// The _provider.
        /// </summary>
        private static ImageProvider _provider;

        /// <summary>
        /// The _providers.
        /// </summary>
        private static ImageProviderCollection _providers;

        /// <summary>
        /// The _photo resize.
        /// </summary>
        private static IDictionary<string, ImageResize> _photoResize;

        /// <summary>
        /// Gets the provider.
        /// </summary>
        public static ImageProvider Provider
        {
            get
            {
                LoadProviders();
                return _provider;
            }
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        public static ImageProviderCollection Providers
        {
            get
            {
                LoadProviders();
                return _providers;
            }
        }

        /// <summary>
        /// Gets the photo resizes.
        /// </summary>
        public static IDictionary<string, ImageResize> ImageResizes
        {
            get
            {
                LoadProviders();
                return _photoResize;
            }
        }

        /// <summary>
        /// The load providers.
        /// </summary>
        /// <exception cref="ProviderException">
        /// </exception>
        private static void LoadProviders()
        {
            if (_provider == null)
            {
                lock (Lock)
                {
                    var section = AppConfig.Instance.Images;

                    _providers = new ImageProviderCollection();
                    ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(ImageProvider));
                    _provider = (ImageProvider) _providers[section.DefaultProvider];

                    _photoResize = new Dictionary<string, ImageResize>();

                    foreach (ImageResizeElement photoResize in section.ImageResizes)
                    {
                        _photoResize.Add(photoResize.Name, new ImageResize(photoResize));
                    }

                    if (_provider == null)
                    {
                        throw new ProviderException("Unable to load default FileSystemProvider");
                    }
                }
            }
        }

        public static bool IsImage(Stream stream)
        {
            if (stream.Length == 0)
            {
                return false;
            }

            var file = System.Drawing.Image.FromStream(stream);

            try
            {
                var allowedFormats = new List<ImageFormat> {ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif};
                return allowedFormats.Contains(file.RawFormat);
            }
            catch (Exception ex)
            {
                Tracing.Error(ex.Message);
            }
            return false;

        }

        #endregion

        #region Provider methods

        /// <summary>
        /// The save photo resize.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="resizeName">
        /// The resize name.
        /// </param>
        /// <returns>
        /// The JamesRocks.Images.Models.Image.
        /// </returns>
        public Image SaveImageResize(ImageRequest item, string resizeName)
        {
            return Provider.SaveImageResize(item, resizeName);
        }

        /// <summary>
        /// The save photo for all sizes.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="tag"></param>
        /// <param name="keepOriginalSize">
        /// The keep original size.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; JamesRocks.Images.Models.Image].
        /// </returns>
        public IList<Image> SaveImageForAllSizes(ImageRequest item, string tag, bool keepOriginalSize = false)
        {
            return Provider.SaveImageForAllSizes(item, tag, keepOriginalSize);
        }

        /// <summary>
        /// The get photo resize.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="resizeName">
        /// The resize name.
        /// </param>
        /// <returns>
        /// The JamesRocks.Images.Models.Image.
        /// </returns>
        public Image GetImageResize(string id, ImageSizeTypeEnum resizeName = ImageSizeTypeEnum.Medium)
        {
            return Provider.GetImageResize(id, resizeName);
        }

        /// <summary>
        /// The get all photo resizes.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IDictionary`2[TKey -&gt; System.String, TValue -&gt; JamesRocks.Images.Models.Image].
        /// </returns>
        public IDictionary<string, Image> GetAllImageResizes(string id)
        {
            return Provider.GetAllImageResizes(id);
        }

        /// <summary>
        /// The get photos by resize.
        /// </summary>
        /// <param name="resizeName">
        /// The resize name.
        /// </param>
        /// <param name="ids">
        /// The ids.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; JamesRocks.Images.Models.Image].
        /// </returns>
        public IList<Image> GetImagesByResize(string resizeName, string[] ids)
        {
            return Provider.GetImagesByResize(resizeName, ids);
        }

        #endregion
    }
}