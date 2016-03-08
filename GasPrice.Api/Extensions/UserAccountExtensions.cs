#region



#endregion

using System;
using System.Web;
using System.Web.Http;
using GasPrice.Core.Account;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Services.Account.Extensions;

// ReSharper disable CheckNamespace

namespace GasPrice.Api.Extensions
{
    #region

    

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public static class UserAccountExtensions
    {

        public static UserAccount Current(this UserAccount account)
        {
            var cache = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IDataCache)) as IDataCache;            
            
            var u = HttpContext.Current.User;
            var userId = u.GetUserId();
            if (cache == null)
            {
                throw new ArgumentNullException("account");
            }

            if (!cache.Contains(DataCacheKey.CurrentUser, userId) )
            {
                var user = Get(userId);
                cache.Insert(DataCacheKey.CurrentUser, userId, user);                    
            }

            return cache.Get<UserAccount>(DataCacheKey.CurrentUser, userId);

        }

        private static UserAccount Get(int userId)
        {
            var userAccountService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUserAccountService)) as IUserAccountService;
            return userAccountService?.GetById(userId);
        }

    }
}

