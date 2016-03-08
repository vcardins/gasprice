using System.Collections.Generic;
using System.Collections.Specialized;
using GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces;

namespace GasPrice.Core.Common.Infrastructure.ImageHandler
{
    public interface IImageProvider
    {
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
        Image SaveImageResize(IImageRequest item, string resizeName);

        /// <summary>
        /// The save photo for all sizes.
        /// </summary>
        /// <param name="photoRequest">The item.</param>
        /// <param name="photoNamePattern">The tag.</param>
        /// <param name="keepOriginalSize">The keep original size.</param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; JamesRocks.Images.Models.Image].
        /// </returns>
        IList<Image> SaveImageForAllSizes(IImageRequest photoRequest, string photoNamePattern, bool keepOriginalSize);

        /// <summary>
        /// Saves the photo for all sizes.
        /// </summary>
        /// <param name="photoRequest">The item.</param>
        /// <param name="container">The container.</param>
        /// <param name="photoNamePattern">Name of the photo unique.</param>
        /// <param name="keepOriginalSize">if set to <c>true</c> [keep original size].</param>
        /// <returns></returns>
        IList<Image> SaveImageForAllSizes(IImageRequest photoRequest, string container, string photoNamePattern, bool keepOriginalSize);

        /// <summary>
        /// The get photo resize.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="sizeName">Name of the size.</param>
        /// <returns>
        /// The JamesRocks.Images.Models.Image.
        /// </returns>
        Image GetImageResize(string id, ImageSizeTypeEnum sizeName);

        /// <summary>
        /// The get all photo resizes.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IDictionary`2[TKey -&gt; System.String, TValue -&gt; JamesRocks.Images.Models.Image].
        /// </returns>
        IDictionary<string, Image> GetAllImageResizes(string id);

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
        IList<Image> GetImagesByResize(string resizeName, string[] ids);

        void Initialize(string name, NameValueCollection config);
        string Name { get; }
        string Description { get; }
    }
}