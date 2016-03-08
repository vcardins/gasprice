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

using GasPrice.Core.Common.Enums;

namespace GasPrice.Core.Config.OAuth
{
    public interface IOAuthSettings
    {
        IOAuthProviderSettings this[OAuthProvider provider] { get; }

    }
}