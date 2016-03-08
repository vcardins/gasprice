/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Deten.Core.Account;
using Deten.Core.Constants;
using Deten.Core.Infrastructure;
using Deten.Services.Account.Extensions;

namespace Deten.Services.Account.Authentication
{
    public abstract class AuthenticationService : IAuthenticationService 
    {
        public IUserAccountService UserAccountService { get; set; }
        public ClaimsAuthenticationManager ClaimsAuthenticationManager { get; set; }

        protected AuthenticationService(IUserAccountService userService)
            : this(userService, null)
        {
        }

        protected AuthenticationService(IUserAccountService userService, 
                                        ClaimsAuthenticationManager claimsAuthenticationManager)
        {
            UserAccountService = userService;
            ClaimsAuthenticationManager = claimsAuthenticationManager;
        }

        protected abstract ClaimsPrincipal GetCurentPrincipal();
        protected abstract void IssueToken(ClaimsPrincipal principal, TimeSpan? tokenLifetime = null, bool? persistentCookie = null);
        protected abstract void RevokeToken();

        public virtual void SignIn(Guid userId, bool persistent = false)
        {
            var account = UserAccountService.GetByID(userId);
            if (account == null) throw new ArgumentException("Invalid userID");

            SignIn(account, AuthenticationMethods.Password, persistent);
        }

        public virtual void SignIn(UserAccount account, bool persistent = false)
        {
            SignIn(account, AuthenticationMethods.Password, persistent);
        }

        public virtual void SignIn(UserAccount account, string method, bool persistent = false)
        {
            if (account == null) throw new ArgumentNullException("account");
            if (String.IsNullOrWhiteSpace(method)) throw new ArgumentNullException("method");

            Tracing.Information("[AuthenticationService.SignIn] sign in called: {0}", account.ID);

            if (!account.IsLoginAllowed || account.IsAccountClosed)
            {
                throw new ValidationException(UserAccountService.GetValidationMessage(AppConstants.ValidationMessages.LoginNotAllowed));
            }

            if (!account.IsAccountVerified && UserAccountService.Configuration.RequireAccountVerification)
            {
                throw new ValidationException(UserAccountService.GetValidationMessage(AppConstants.ValidationMessages.AccountNotVerified));
            }

            // gather claims
            var claims = GetAllClaims(account, method);
            
            // get custom claims from properties
            claims.AddRange(UserAccountService.MapClaims(account));

            // create principal/identity
            var id = new ClaimsIdentity(claims, method);
            var cp = new ClaimsPrincipal(id);

            // claims transform
            if (ClaimsAuthenticationManager != null)
            {
                cp = ClaimsAuthenticationManager.Authenticate(String.Empty, cp);
            }

            // issue cookie
            Tracing.Verbose("[AuthenticationService.SignIn] token issued: {0}", account.ID);
            IssueToken(cp, persistentCookie: persistent);
        }

        private static List<Claim> GetBasicClaims(UserAccount account, string method)
        {
            if (account == null) throw new ArgumentNullException("account");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.AuthenticationMethod, method),
                new Claim(ClaimTypes.AuthenticationInstant, DateTime.Now.ToString("s"))
            };
            claims.AddRange(account.GetIdentificationClaims());

            return claims;
        }
        
        private static List<Claim> GetAllClaims(UserAccount account, string method)
        {
            if (account == null) throw new ArgumentNullException("account");

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, method));
            claims.Add(new Claim(ClaimTypes.AuthenticationInstant, DateTime.Now.ToString("s")));
            claims.AddRange(account.GetAllClaims());

            return claims;
        }

        public virtual void SignOut()
        {
            var p = GetCurentPrincipal();
            if (p.HasUserId())
            {
                Tracing.Information("[AuthenticationService.SignOut] called: {0}", p.GetUserID());
            }

            // clear cookie
            RevokeToken();
        }
    }
    
}
