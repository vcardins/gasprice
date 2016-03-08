﻿/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Security.Claims;

namespace GasPrice.Services.Account.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static bool HasClaim(this ClaimsIdentity user, string type)
        {
            if (user != null)
            {
                return user.HasClaim(x => x.Type == type);
            }
            return false;
        }

        public static Guid GetUserID(this ClaimsIdentity user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var id = user.Claims.GetValue(ClaimTypes.NameIdentifier);
            Guid g;
            if (Guid.TryParse(id, out g))
            {
                return g;
            }

            throw new Exception("Invalid NameIdentifier");
        }

        public static int GetUserId(this ClaimsIdentity user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var id = user.Claims.GetValue(ClaimTypes.NameIdentifier);
            int g;
            if (Int32.TryParse(id, out g))
            {
                return g;
            }
            
            throw new Exception("Invalid NameIdentifier");
        }

        public static bool HasUserID(this ClaimsIdentity user)
        {
            if (user != null)
            {
                var id = user.Claims.GetValue(ClaimTypes.NameIdentifier);
                Guid g;
                if (Guid.TryParse(id, out g))
                {
                    return true;
                }
            }
            
            return false;
        }

        public static bool HasUserId(this ClaimsIdentity user)
        {
            if (user != null)
            {
                var id = user.Claims.GetValue(ClaimTypes.NameIdentifier);
                int g;
                if (Int32.TryParse(id, out g))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
