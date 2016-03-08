using GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces;

namespace GasPrice.Core.Common.Infrastructure.ImageHandler
{
    /// <summary>
    /// The photo resize.
    /// </summary>
    public class ImageResize
    {
        /// <summary>
        /// The name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The width.
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// The height.
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// The height.
        /// </summary>
        public readonly bool Enabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageResize"/> class.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        public ImageResize(IImageResize element)
        {
            Name = element.Name;
            Width = element.Width;
            Height = element.Height;
            Enabled = element.Enabled;
        }
    }
}