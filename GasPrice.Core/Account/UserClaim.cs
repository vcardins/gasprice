/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GasPrice.Core.Account
{
    public class UserClaim : UserKey
    {
        public UserClaim()
        {
        }
        
        public UserClaim(string type, string value)
        {
            if (String.IsNullOrWhiteSpace(type)) throw new ArgumentNullException("type");
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value");

            Type = type;
            Value = value;
        }

        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
    }

    public static class UserClaimCollectionExtensions
    {
        public static UserClaimCollection ToCollection(this IEnumerable<UserClaim> claims)
        {
            return new UserClaimCollection(claims);
        }
    }

    public class UserClaimCollection : HashSet<UserClaim>
    {
        public static readonly UserClaimCollection Empty = new UserClaimCollection();

        public static implicit operator UserClaimCollection(UserClaim[] claims)
        {
            return new UserClaimCollection(claims);
        }
        
        public static implicit operator UserClaimCollection(Claim[] claims)
        {
            return new UserClaimCollection(claims);
        }

        public UserClaimCollection()
        {
        }

        public UserClaimCollection(IEnumerable<UserClaim> claims)
        {
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    Add(claim);
                }
            }
        }
        public UserClaimCollection(IEnumerable<Claim> claims)
        {
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    Add(claim.Type, claim.Value);
                }
            }
        }

        public void Add(string type, string value)
        {
            Add(new UserClaim(type, value));
        }
    }
}
