using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GasPrice.Api.Filters
{

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ClaimsDeauthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _action;
        private readonly string[] _resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsAuthorizeAttribute"/> class.
        /// </summary>
        public ClaimsDeauthorizeAttribute()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="resources">The resources.</param>
        public ClaimsDeauthorizeAttribute(string action, params string[] resources)
        {
            _action = action;
            _resources = resources;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
            var claim = identity.Claims.FirstOrDefault(c => c.Type == _action && _resources.Contains(c.Value));
            return claim == null;
        }

     
    }
    
}