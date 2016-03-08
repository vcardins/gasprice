using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace GasPrice.Api.Providers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationProvider" />
    public class OAuthBearerTokenProvider : OAuthBearerAuthenticationProvider
    {
        private readonly List<Func<IOwinRequest, string>> _locations;
        private readonly Regex _bearerRegex = new Regex("((B|b)earer\\s)");
        private const string AuthHeader = "Authorization";
        /// <summary>
        /// By Default the Token will be searched for on the "Authorization" header.
        /// <para> pass additional getters that might return a token string</para>
        /// </summary>
        /// <param name="locations"></param>
        public OAuthBearerTokenProvider(params Func<IOwinRequest, string>[] locations)
        {
            _locations = locations.ToList();
            //Header is used by default
            _locations.Add(x => x.Headers.Get(AuthHeader));            
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var getter = _locations.FirstOrDefault(x => !String.IsNullOrWhiteSpace(x(context.Request)));
            if (getter != null)
            {
                var tokenStr = getter(context.Request);
                context.Token = _bearerRegex.Replace(tokenStr, "").Trim();
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            var claims = context.Ticket.Identity.Claims;
            var enumerable = claims as IList<Claim> ?? claims.ToList();
            if (!enumerable.Any() || enumerable.Any(claim => claim.Issuer != ClaimsIdentity.DefaultIssuer))
            {
                context.Rejected();
            }

            return Task.FromResult<object>(null);
        }
    }
}