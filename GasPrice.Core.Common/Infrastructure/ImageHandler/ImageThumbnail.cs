// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhotoThumbnail.cs" company="">
//   
// </copyright>
// <summary>
//   The photo thumbnail.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GasPrice.Core.Common.Infrastructure.ImageHandler
{
    /// <summary>
    /// The photo thumbnail.
    /// </summary>
    public class ImageThumbnail
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the photo id.
        /// </summary>
        public int PhotoID { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the image size type value.
        /// </summary>
        public int ImageSizeTypeValue { get; set; }

        /// <summary>
        /// Gets or sets the image size type.
        /// </summary>
        public ImageSizeTypeEnum ImageSizeType
        {
            get
            {
                return (ImageSizeTypeEnum)ImageSizeTypeValue;
            }

            set
            {
                ImageSizeTypeValue = (int)value;
            }
        }
    }
}