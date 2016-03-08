using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using GasPrice.Core.Extensions;
using GasPrice.Core.Account;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Common.Infrastructure;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace GasPrice.Api.Providers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerProvider" />
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IDependencyResolver _resolver;
        private readonly string _publicClientId;
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationOAuthProvider"/> class.
        /// </summary>
        public ApplicationOAuthProvider()
        {
            _resolver = GlobalConfiguration.Configuration.DependencyResolver;
            _publicClientId = null;
        }
        
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
       
            var userService = _resolver.GetService(typeof(IUserAccountService)) as IUserAccountService;
            if (userService == null)
            {
                context.SetError("invalid_service", "Missing authentication service");
                return;
            }
            
            var username = context.UserName;

            UserAccountAuthentication account;
            if (!await userService.Authenticate(username, context.Password, out account))
            {
                var msg = account.Status.HasValue
                    ? account.Status.GetDescription()
                    : "An error occurred trying to authentication your account.";
                context.SetError("invalid_grant", msg);
                return;
            }
          
            var authenticationManager = context.Request.Context.Authentication;
            authenticationManager.SignOut(context.Options.AuthenticationType);

            var roles = new List<string>();
            var claims = account.Claims ?? new List<Claim>();
            var access = new AccessRole();

            roles.AddRange(from claim in claims where claim.Type == ClaimTypes.Role select claim.Value);
            if (roles.Count == 0)
            {
                access.Name = UserRoleEnum.Reader;
                roles.Add(access.Name.ToString());
            }
            else
            {
                access.Name = (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), roles[0]);
            }
            
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password));
            claims.Add(new Claim(ClaimTypes.AuthenticationInstant, DateTime.Now.ToString("s")));
            claims.Add(new Claim(ClaimTypes.Name, account.Username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim("Guid", account.ID.ToString()));

            var cookiesIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
            var oAuthIdentity = new ClaimsIdentity(claims, context.Options.AuthenticationType);
            var currentUtc = new SystemClock().UtcNow;

            var properties = CreateProperties(context.ClientId, username, access);
            properties.IssuedUtc = currentUtc;
            properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
            
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);

            context.Validated(ticket);
            authenticationManager.SignIn(cookiesIdentity);

            Thread.CurrentPrincipal = new GenericPrincipal(oAuthIdentity, new string[0]);

            CacheUserAccount(account);
        }

        private void CacheUserAccount(UserAccountAuthentication account)
        {
            var cache = _resolver.GetService(typeof(IDataCache)) as IDataCache;
            cache?.Insert(DataCacheKey.CurrentUser, account.UserId, account);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Valildate the redirect uri
        /// </summary>
        /// <param name="context">Validate context</param>
        /// <returns>Task</returns>		
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Password resource owner credentials don´t provide a client identifier
        /// </summary>
        /// <param name="context">Validate context</param>
        /// <returns>Task</returns>		
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.OwinContext.Set("as:clientAllowedOrigin", "http://localhost:3064");
                context.OwinContext.Set("as:clientRefreshTokenLifeTime", 7200);
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }
        /*
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (string.IsNullOrEmpty(context.ClientId))
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrets once obtain access tokens. 
                context.Validated();
                context.SetError("invalid_clientId", "Client Id is required.");
                return;
            }
            
            var audienceService = _resolver.GetService(typeof(IAudienceService)) as IAudienceService;
            if (audienceService == null)
            {
                throw new ArgumentNullException("context");
            }

            var client = await audienceService.GetAudience(Guid.Parse(context.ClientId));
            if (client == null)
            {
                context.SetError("invalid_clientId", "Client Id is invalid.");
                return;
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client Id is invalid.");
                    return;
                }
                var cryptoService = _resolver.GetService(typeof(ICrypto)) as ICrypto;
                if (cryptoService == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (client.Secret != cryptoService.Hash(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                    return;
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return;
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();

        }
        */

        /// <summary>
        /// Creates the properties.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public static AuthenticationProperties CreateProperties(string clientId, string userName, AccessRole role) //string[] roles
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "as:client_id", clientId ?? string.Empty },
                { "username", userName },
                { "role", role.Name.ToString().ToLower() },
                { "bitMask", role.BitMask.ToString() },
                { "success", true.ToString() }
            };
            return new AuthenticationProperties(data);
        }
    }
}