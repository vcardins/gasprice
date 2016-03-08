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
    public interface IImageSettings
    {
        string DefaultAvatar { set; get; }
        string DefaultAvatarPicture { set; get; }
        string ImageCacheUrl { set; get; }
        string TempCacheUrl { set; get; }

        
    }
}