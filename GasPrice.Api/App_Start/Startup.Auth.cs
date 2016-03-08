using System;
using GasPrice.Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace GasPrice.Api
{
    public partial class Startup {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        /// <summary>
        /// Gets the o authentication server options.
        /// </summary>
        /// <value>
        /// The o authentication server options.
        /// </value>
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
        /// <summary>
        /// Gets the o authentication bearer options.
        /// </summary>
        /// <value>
        /// The o authentication bearer options.
        /// </value>
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        /// <summary>
        /// Configurations the o authentication.
        /// </summary>
        /// <param name="app">The application.</param>
        public void ConfigOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");

            OAuthBearerOptions = new OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = "Bearer",
                Provider = new OAuthBearerTokenProvider()
            };

            OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(15), //Token expiration => The user will remain authenticated for 14 days
                Provider = new ApplicationOAuthProvider()
            };

            // Enable the application to use bearer tokens to authenticate users
            // Enabling 3 components:
            // 1. Authorization Server middleware. For creating the bearer tokens
            // 2. Application bearer token middleware. Will atuthenticate every request with Authorization : Bearer header
            // 3. External bearer token middleware. For external providers
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            //app.UseOAuthBearerTokens(OAuthServerOptions, OAuthServerOptions.AuthenticationType);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = OAuthBearerOptions.AuthenticationType,
                AuthenticationMode = AuthenticationMode.Active
            });

        }
    }
}