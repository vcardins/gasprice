using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GasPrice.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class ArrayInputAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;
        private readonly Type _parameterType;

        public ArrayInputAttribute(string parameterName, Type parameterType)
        {
            _parameterName = parameterName;
            _parameterType = parameterType;
            Separator = ',';
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.ContainsKey(_parameterName))
            {
                var parameters = string.Empty;
                if (actionContext.ControllerContext.RouteData.Values.ContainsKey(_parameterName))
                    parameters = (string)actionContext.ControllerContext.RouteData.Values[_parameterName];
                else if (actionContext.ControllerContext.Request.RequestUri.ParseQueryString()[_parameterName] != null)
                    parameters = actionContext.ControllerContext.Request.RequestUri.ParseQueryString()[_parameterName];

                parameters = HttpUtility.UrlDecode(parameters);
                if (string.IsNullOrEmpty(parameters))
                    return;

                if (_parameterType == typeof(int))
                    actionContext.ActionArguments[_parameterName] = parameters.Split(Separator).Select(int.Parse).ToList();
                else
                    actionContext.ActionArguments[_parameterName] = parameters.Split(Separator).ToList();
            }
        }

        public char Separator { get; set; }

    }
}