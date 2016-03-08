/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */


using System;
using System.Collections.Generic;
using System.Security.Claims;
using GasPrice.Core.Account;

namespace GasPrice.Services.Account.Extensions
{
    public static class UserAccountServiceExtensions
    {

        public static void AddClaims(this IUserAccountService svc, Guid accountID, IEnumerable<Claim> claims)
            
        {
            if (svc == null) throw new ArgumentNullException("account");

            svc.AddClaims(accountID, new UserClaimCollection(claims));
        }

        public static void RemoveClaims(this IUserAccountService svc, Guid accountID, IEnumerable<Claim> claims)
            
        {
            if (svc == null) throw new ArgumentNullException("account");

            svc.RemoveClaims(accountID, new UserClaimCollection(claims));
        }
        
        public static void UpdateClaims(this IUserAccountService svc, Guid accountID, IEnumerable<Claim> additions = null, IEnumerable<Claim> deletions = null)
            
        {
            if (svc == null) throw new ArgumentNullException("account");

            svc.UpdateClaims(accountID, new UserClaimCollection(additions), new UserClaimCollection(deletions));
        }
    }
}
