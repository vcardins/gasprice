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

namespace GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces
{
    #region

    

    #endregion

    public interface IImageResize
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        bool Enabled { get; set; }
    }
}