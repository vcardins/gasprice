using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.Filters;
using GasPrice.Core.Models.Infraestructure;
using GasPrice.Services.Account.Extensions;

namespace GasPrice.Api.Filters
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class LogAttribute : ActionFilterAttribute
    {

        private IRepositoryAsync<Log> _logRepo;

        /// <summary>
        /// Action being executed
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public ModelAction Action { get; set; }

        /// <summary>
        /// Module which the log will be registered against
        /// </summary>

         // This constructor specifies the unnamed arguments to the attribute class.
        /// <summary>
        /// Initializes a new instance of the <see cref="LogAttribute"/> class.
        /// </summary>
        public LogAttribute()
        {
            Action = ModelAction.Unknown;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //_logger.InfoFormat(CultureInfo.InvariantCulture, 
            //    "Executing action {0}.{1}", 
            //    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
            //    filterContext.ActionDescriptor.ActionName);
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var method = context.Request.Method?.Method;
            var controller = context.Request.GetActionDescriptor().ControllerDescriptor;            

            if (method != null && method.Equals("GET")) return;

            var resolver = GlobalConfiguration.Configuration.DependencyResolver;
            _logRepo = (IRepositoryAsync<Log>) resolver.GetService(typeof(IRepositoryAsync<Log>));

            var owinContext = context.Request.GetOwinContext();
            int? userId = null;
            if (owinContext.Authentication.User.Identity.IsAuthenticated)
            {
                userId = owinContext.Authentication.User.GetUserId();
            }

            var msg = string.Empty;
            var attrs = controller.GetType().GetCustomAttributes(typeof (ModuleAttribute), true).FirstOrDefault();
            var attribute = (ModuleAttribute)controller.GetType().GetCustomAttributes(typeof(ModuleAttribute), true).FirstOrDefault();
            var module = attribute?.Name ?? ModelType.Unknown;

            var log = new Log
            {
                UserId = userId,
                Message = msg,
                Module = module,
                Type = method,
                Action = Action,
                Source = controller.ControllerName,
                Created = DateTime.Now
            };

            _logRepo.InsertAsync(log, true);

        }
   
    }
}