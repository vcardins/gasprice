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

using System.Collections.Generic;

namespace GasPrice.Core.Config.OAuth
{
    #region

    

    #endregion

    public interface IOAuthProviderCollection : IEnumerable<IOAuthProviderSettings>
    {
        //IOAuthProviderSettings Provider { get; set; }
    }
}