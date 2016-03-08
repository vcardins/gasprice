using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using GasPrice.Core.Constants;
using GasPrice.Core.Filters;

namespace GasPrice.Services.Account.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasClaim(this ClaimsPrincipal user, string type)
        {
            if (user != null)
            {
                return user.HasClaim(x => x.Type == type);
            }
            return false;
        }

        public static Guid GetUserID(this IPrincipal p)
        {
            var cp = p as ClaimsPrincipal;
            if (cp != null)
            {
                var id = cp.Claims.GetValue("Guid");
                Guid g;
                if (Guid.TryParse(id, out g))
                {
                    return g;
                }
            }
            throw new Exception(UserAccountConstants.InformationMessages.InvalidNameIdentifier);
        }

        public static int GetUserId(this IPrincipal p)
        {
            var cp = p as ClaimsPrincipal;
            if (cp != null)
            {
                var id = cp.Claims.GetValue(ClaimTypes.NameIdentifier);
                int g;
                if (Int32.TryParse(id, out g))
                {
                    return g;
                }
            }
            throw new Exception(UserAccountConstants.InformationMessages.InvalidNameIdentifier);
        }

        public static bool IsAdmin(this IPrincipal p)
        {
            var cp = p as ClaimsPrincipal;
            if (cp == null) throw new Exception(UserAccountConstants.InformationMessages.InvalidNameIdentifier);
            return cp.IsInRole("Admin");
        }

        public static void CheckAccess(this IPrincipal p, string action, string[] resources)
        {
            var cp = p as ClaimsPrincipal;
            if (cp == null) throw new Exception(UserAccountConstants.InformationMessages.InvalidNameIdentifier);

            var claim = cp.Claims.FirstOrDefault(c => c.Type == action && resources.Contains(c.Value));
            if (claim == null)
            {
                throw new ForbiddenException();
            }
        }

        public static bool HasUserId(this IPrincipal p)
        {
            var cp = p as ClaimsPrincipal;
            if (cp == null) return false;
            var id = cp.Claims.GetValue(ClaimTypes.NameIdentifier);
            int g;
            if (Int32.TryParse(id, out g))
            {
                return true;
            }
            return false;
        }

        public static bool HasUserID(this IPrincipal p)
        {
            var cp = p as ClaimsPrincipal;
            if (cp != null)
            {
                var id = cp.Claims.GetValue(ClaimTypes.NameIdentifier);
                Guid g;
                if (Guid.TryParse(id, out g))
                {
                    return true;
                }
            }
            return false;
        }
      
        public static string GetUserName(this IPrincipal p)
        {
            var cp = p as ClaimsPrincipal;
            if (cp == null)
                throw new Exception("Invalid Name");

            var username = cp.Claims.GetValue(ClaimTypes.Name);
            return username;
        }

        public static string GetClaimValue(this IPrincipal cp, string key)
        {
            var identity = cp.Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.First(c => c.Type == key);
            return claim.Value;
        }

        public static string FindFirstValue(this ClaimsIdentity i, string value)
        {
            if (i != null)
            {
                var claim = i.Claims.GetValue(value);
                return claim.FirstOrDefault().ToString();
            }
            throw new Exception("Invalid Claim value");
        }

    }
}