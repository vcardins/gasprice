/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;

namespace GasPrice.Services.Account.Extensions
{
    public static class DisposableExtensions
    {
        public static bool TryDispose(this IDisposable item)
        {
            if (item == null) return false;
            item.Dispose();
            return true;
        }
    }
}
