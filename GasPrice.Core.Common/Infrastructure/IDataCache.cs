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
    public interface IDataCache
    {
        void Insert<T>(DataCacheKey key, T value);
        void Insert<T>(DataCacheKey key, int subKey, T value);
        T Get<T>(DataCacheKey key);
        T Get<T>(DataCacheKey key, int subKey);
        bool Contains(DataCacheKey key);
        bool Contains(DataCacheKey key, int subKey);
        void Remove(DataCacheKey key);
        void Remove(DataCacheKey key, int subKey);
    }
}
