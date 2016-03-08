

namespace GasPrice.Core.Common.Infrastructure.ImageHandler
{
    /// <summary>
    /// The photo.
    /// </summary>
    public partial class Image
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>       
        /// todo: "Id" should only be reserved for fields with a database id.  Use 'ImageCode' instead.
        public string ImageId { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the resize name.
        /// </summary>
        public string ResizeName { get; set; }

        /// <summary>
        /// Gets or sets the resize name.
        /// </summary>
        public ImageResize Size { get; set; }

    }
}