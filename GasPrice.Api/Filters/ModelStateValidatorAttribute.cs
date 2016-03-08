using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using GasPrice.Api.Helpers;

namespace GasPrice.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ModelStateValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var el = actionContext.ActionArguments.Values.ElementAtOrDefault(0);
            if (el == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest,
                    new { Message = "Model contains no values" });
            }
            var modelState = actionContext.ModelState;
            if (modelState.IsValid) {
                return;
            }
            IEnumerable errors = modelState.Errors();
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
        }      

    }
}
