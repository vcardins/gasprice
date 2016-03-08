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

using System;
using System.Web;

// ReSharper disable CheckNamespace

namespace GasPrice.Api.Extensions
{
    #region

    

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public static partial class HttpContextExtensions
    {
        /// <summary>
        /// Gets the application URL.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">ctx</exception>
        public static string GetApplicationUrl(this HttpContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");

            return new HttpContextWrapper(ctx).GetApplicationUrl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetApplicationUrl(this HttpContextBase ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");

            return ctx.Request.GetApplicationUrl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetApplicationUrl(this HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");

            var baseUrl = request.Url.Scheme + "://" + request.Url.Host + (request.Url.Port == 80 ? "" : ":" + request.Url.Port) + request.ApplicationPath;
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            return baseUrl;
        }
    }
}

