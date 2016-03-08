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

namespace GasPrice.Infrastructure.Extensions
{
    #region

    

    #endregion

    public static class HttpContextExtensions
    {
        public static string GetApplicationUrl(this HttpContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");

            return new HttpContextWrapper(ctx).GetApplicationUrl();
        }

        public static string GetApplicationUrl(this HttpContextBase ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");

            return ctx.Request.GetApplicationUrl();
        }

        public static string GetApplicationUrl(this HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");

            var baseUrl =
                request.Url.Scheme +
                "://" +
                request.Url.Host + (request.Url.Port == 80 ? "" : ":" + request.Url.Port) +
                request.ApplicationPath;
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            return baseUrl;
        }
    }
}

