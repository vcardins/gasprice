#region credits
// ***********************************************************************
// Assembly	: AngularJSAuth.Core
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion
namespace GasPrice.Core.Common.Infrastructure
{
    public interface IAppContext
    {
        string GetIP();
        string GetUserAgent();
        bool IsMobile();
        string GetDeviceId();

    }
}
