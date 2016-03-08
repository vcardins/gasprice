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

using GasPrice.Core.Common.Infrastructure;

namespace GasPrice.Infrastructure.Caching
{
    #region

    

    #endregion

    /// <summary>
    /// Library used for arbitrary caching of objects
    /// </summary>
    public class MyCache : IDataCache
    {
        public void Insert<T>(DataCacheKey key, T value)
        {
            DataCache.Insert(key, value);
        }

        public void Insert<T>(DataCacheKey key, int subKey, T value)
        {
            DataCache.Insert(key, subKey, value);
        }

        public T Get<T>(DataCacheKey key)
        {
            return DataCache.Get<T>(key);
        }

        public T Get<T>(DataCacheKey key, int subKey)
        {
            return DataCache.Get<T>(key, subKey);
        }

        public bool Contains(DataCacheKey key)
        {
            return DataCache.Contains(key);
        }

        public bool Contains(DataCacheKey key, int subKey)
        {
            return DataCache.Contains(key, subKey);
        }

        public void Remove(DataCacheKey key)
        {
            DataCache.Remove(key);
        }

        public void Remove(DataCacheKey key, int subKey)
        {
            DataCache.Remove(key, subKey);
        }

    }
}
