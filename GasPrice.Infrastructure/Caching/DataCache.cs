#region credits
// ***********************************************************************
// Assembly	: Deten.Infrastructure
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-21-2013
// ***********************************************************************
#endregion

using System;
using System.Runtime.Caching;
using GasPrice.Core.Common.Infrastructure;

namespace GasPrice.Infrastructure.Caching
{
    #region

    

    #endregion

    /// <summary>
    /// Library used for arbitrary caching of objects
    /// </summary>
    public class DataCache
    {
        public static void Insert<T>(DataCacheKey key, T value)
        {
            lock (MemoryCache.Default)
            {
                const long min = 60;
                MemoryCache.Default.Remove(key.ToString());
                var expires = DateTime.Now.AddMinutes(min);
                MemoryCache.Default.Add(key.ToString(), value, expires);
            }
        }

        public static void Insert<T>(DataCacheKey key, int subKey, T value)
        {
            lock (MemoryCache.Default)
            {
                var k = GetKey(key, subKey);
                const int min = 60;
                MemoryCache.Default.Remove(k);
                var expires = DateTime.Now.AddMinutes(min);
                MemoryCache.Default.Add(k, value, expires);
            }
        }

        public static T Get<T>(DataCacheKey key)
        {
            if (MemoryCache.Default.Contains(key.ToString()))
            {
                return (T)MemoryCache.Default.Get(key.ToString());
            }
            return default(T);
        }

        public static T Get<T>(DataCacheKey key, int subKey)
        {
            var k = GetKey(key, subKey);
            if (MemoryCache.Default.Contains(k))
            {
                return (T)MemoryCache.Default.Get(k);
            }
            return default(T);
        }

        public static bool Contains(DataCacheKey key)
        {
            return MemoryCache.Default.Contains(key.ToString());
        }

        public static bool Contains(DataCacheKey key, int subKey)
        {
            return MemoryCache.Default.Contains(GetKey(key, subKey));
        }

        public static void Remove(DataCacheKey key)
        {
            MemoryCache.Default.Remove(key.ToString());
        }

        public static void Remove(DataCacheKey key, int subKey)
        {
            MemoryCache.Default.Remove( GetKey(key, subKey) );
        }

        private static string GetKey(DataCacheKey key, int subKey)
        {
            return string.Format("{0}-{1}", key, subKey);
        }
    }
}
