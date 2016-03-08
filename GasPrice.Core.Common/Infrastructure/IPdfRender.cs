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

namespace GasPrice.Core.Common.Infrastructure
{
    public interface IPdfRenderer
    {
        int HorizontalMargin { get; set; }

        int VerticalMargin { get; set; }

        byte[] RenderAsBytes(string htmlText, string pageTitle);

        MemoryStream Render(string htmlText, string pageTitle);
    }
}
