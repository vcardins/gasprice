/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using GasPrice.Core.Account;
using GasPrice.Core.Constants;

namespace GasPrice.Services.Account.Extensions
{
    public static class UserAccountExtensions
    {
        public static bool HasClaim(this UserAccount account, string type)
        {
            if (account == null) throw new ArgumentException("account");
            if (String.IsNullOrWhiteSpace(type)) throw new ArgumentException("type");

            return account.Claims.Any(x => x.Type == type);
        }

        public static bool HasClaim(this UserAccount account, string type, string value)
        {
            if (account == null) throw new ArgumentException("account");
            if (String.IsNullOrWhiteSpace(type)) throw new ArgumentException("type");
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("value");

            return account.Claims.Any(x => x.Type == type && x.Value == value);
        }

        public static IEnumerable<string> GetClaimValues(this UserAccount account, string type)
        {
            if (account == null) throw new ArgumentException("account");
            if (String.IsNullOrWhiteSpace(type)) throw new ArgumentException("type");

            var query =
                from claim in account.Claims
                where claim.Type == type
                select claim.Value;
            return query.ToArray();
        }

        public static string GetClaimValue(this UserAccount account, string type)
        {
            if (account == null) throw new ArgumentException("account");
            if (String.IsNullOrWhiteSpace(type)) throw new ArgumentException("type");

            var query =
                from claim in account.Claims
                where claim.Type == type
                select claim.Value;
            return query.SingleOrDefault();
        }

        public static bool HasPassword(this UserAccount account)
        {
            if (account == null) throw new ArgumentException("account");
            return !String.IsNullOrWhiteSpace(account.HashedPassword);
        }
        
        public static bool IsNew(this UserAccount account)
        {
            if (account == null) throw new ArgumentException("account");
            return account.LastLogin == null;
        }

        public static IEnumerable<Claim> GetIdentificationClaims(this UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.ID.ToString("D")),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(UserAccountConstants.ClaimTypes.Tenant, account.Tenant)
            };

            return claims;
        }

        public static IEnumerable<Claim> GetAllClaims(this UserAccount account)
        {
            if (account == null) throw new ArgumentNullException("account");

            var claims = new List<Claim>();
            claims.AddRange(account.GetIdentificationClaims());

            if (!String.IsNullOrWhiteSpace(account.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, account.Email));
            }
            var otherClaims =
                (from uc in account.Claims
                 select new Claim(uc.Type, uc.Value)).ToList();
            claims.AddRange(otherClaims); 
            
            return claims;
        }
    }
}
