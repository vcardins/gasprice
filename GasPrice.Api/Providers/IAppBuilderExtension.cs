using Microsoft.Owin.Security.OAuth;
using Owin;

namespace GasPrice.Api.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public static class IAppBuilderExtension
    {
        /// <summary>
        /// Uses the o authentication bearer tokens.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="options">The options.</param>
        /// <param name="externalAuthenticationType">Type of the external authentication.</param>
        public static void UseOAuthBearerTokens(this IAppBuilder app, OAuthAuthorizationServerOptions options, string externalAuthenticationType)
        {
            var bearerAuthenticationProvider = new OAuthBearerTokenProvider();

            app.UseOAuthAuthorizationServer(options);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = options.AccessTokenFormat,
                AccessTokenProvider = options.AccessTokenProvider,
                AuthenticationMode = options.AuthenticationMode,
                AuthenticationType = options.AuthenticationType,
                Description = options.Description,
                Provider = bearerAuthenticationProvider,
                SystemClock = options.SystemClock
            });

        }

    }
}