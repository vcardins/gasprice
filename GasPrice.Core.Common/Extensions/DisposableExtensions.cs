#region credits
// ***********************************************************************
// Assembly	: GasPrice.Common
// Author	: Victor Cardins
// Created	: 03-23-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion
#region



#endregion

// ReSharper disable CheckNamespace
using System;

namespace GasPrice.Core.Extensions
{
    #region

    

    #endregion

    public static class DisposableExtensions
        // ReSharper restore CheckNamespace
    {
        public static bool TryDispose(this IDisposable item)
        {
            if (item == null) return false;
            item.Dispose();
            return true;
        }
    }
}

