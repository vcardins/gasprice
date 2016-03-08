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

using System.IO;
using GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces;

namespace GasPrice.Infrastructure.ImageHandler
{
    #region

    

    #endregion

    /// <summary>
    /// The photo request.
    /// </summary>
    public class ImageRequest : IImageRequest
    {
        /// <summary>
        /// Gets the stream.
        /// </summary>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Gets the mime type.
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// Gets the photo id.
        /// </summary>
        public string ImageID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRequest"/> class.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="mimeType">
        /// The mime type.
        /// </param>
        /// <param name="photoId">
        /// The photo id.
        /// </param>
        public ImageRequest(Stream stream, string mimeType, string photoId)
        {
            Stream = stream;
            MimeType = mimeType;
            ImageID = photoId;
        }

        //public ImageRequest(HttpPostedFileBase file, string photoId) : this(file.InputStream, file.ContentType, photoId)
        //{
        //}
    }
}