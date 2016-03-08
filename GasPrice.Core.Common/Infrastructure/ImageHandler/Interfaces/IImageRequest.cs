#region credits
// ***********************************************************************
// Assembly	: GasPrice.Core
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System.IO;

namespace GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces
{
    #region

    

    #endregion

    public interface IImageRequest
    {
        /// <summary>
        /// Gets the stream.
        /// </summary>
        Stream Stream { get; }

        /// <summary>
        /// Gets the mime type.
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Gets the photo id.
        /// </summary>
        string ImageID { get; }
    }
}