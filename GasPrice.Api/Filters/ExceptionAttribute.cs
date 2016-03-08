using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using GasPrice.Core.Filters;
using GasPrice.Core.Models.Infraestructure;
using GasPrice.Core.Services;
using GasPrice.Services.Account.Extensions;

namespace GasPrice.Api.Filters
{

    /// <summary>
    /// 
    /// </summary>
    public class ExceptionAttribute : ExceptionFilterAttribute
    {

        //Populate Exception message within constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionAttribute"/> class.
        /// </summary>
        public ExceptionAttribute()
        {
            Mappings = new Dictionary<Type, HttpStatusCode>
            {
                {typeof (ArgumentNullException), HttpStatusCode.BadRequest},
                {typeof (ArgumentException), HttpStatusCode.BadRequest},
                {typeof (IndexOutOfRangeException), HttpStatusCode.BadRequest},
                {typeof (DivideByZeroException), HttpStatusCode.BadRequest},
                {typeof (InvalidOperationException), HttpStatusCode.BadRequest},
                {typeof (ValidationException), HttpStatusCode.BadRequest},
                {typeof (CustomValidationException), HttpStatusCode.BadRequest},
                {typeof (DbUpdateException), HttpStatusCode.InternalServerError},
                {typeof (ForbiddenException), HttpStatusCode.Forbidden},
                {typeof (NotFoundException), HttpStatusCode.NotFound},
                {typeof (NotImplementedException), HttpStatusCode.BadRequest}                
            };
        }

        /// <summary>
        /// Gets the mappings.
        /// </summary>
        /// <value>
        /// The mappings.
        /// </value>
        public IDictionary<Type, HttpStatusCode> Mappings
        {
            get;
            //Set is private to make it singleton
            private set;
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context == null)
                return;

            var exception = context.Exception;
            if (exception == null) 
                return;

            var type = exception.GetType();

            HttpStatusCode? httpStatusCode = null;
            var reason = "Error";
            var externalMessage = context.Exception.Message;
            var internalMessage = externalMessage;            

            if (Mappings.ContainsKey(type))
            {
                httpStatusCode = Mappings[exception.GetType()];
                if (type == typeof (DbUpdateException))
                {
                    internalMessage = exception.InnerException.InnerException !=null ? 
                        exception.InnerException.InnerException.Message :
                        exception.InnerException.Message;
                }
            }
            else 
            { 
                if (exception is NotImplementedException)
                {
                    externalMessage = exception.Message;
                    httpStatusCode = HttpStatusCode.NotImplemented;
                    reason = "Not Implemented";                               
                }                
            }

            //"System failed to process request"
            //Else part executes means there is not information in repository so it is some kind of anonymous exception
            if (!httpStatusCode.HasValue) {
                //message = "Sorry, an error has occurred. System administrator will be informed";
                httpStatusCode = HttpStatusCode.InternalServerError;
                reason = "System Failure";
            }

            if (!(exception is CustomValidationException) && 
                !(exception is ForbiddenException) && 
                !(exception is NotFoundException)
               )
            {
                Task.Run(() => LogError(context, internalMessage));
            }
            
            throw new HttpResponseException(new HttpResponseMessage(httpStatusCode.Value)
            {
                Content = JsonError(externalMessage),
                ReasonPhrase = reason
            });
          
        }

        private static StringContent JsonError(string err, string title = "")
        {
            return new StringContent("{\"message\":\"" + (title + " " + err).Trim() + "\"}", Encoding.UTF8, "application/json");
        }
        
        private static async Task LogError(HttpActionExecutedContext context, string message)
        {
            var owinContext = context.Request.GetOwinContext();
            int? userId = null;
            if (owinContext.Authentication.User.Identity.IsAuthenticated)
            {
                userId = owinContext.Authentication.User.GetUserId();    
            }

            var err = new ExceptionLog
            {
                UserId = userId,
                Message = message,
                Source = context.Exception.Source,
                StackTrace = context.Exception.StackTrace,
                HResult = context.Exception.HResult,
                RequestUri = context.Request.RequestUri.AbsolutePath,
                Method = context.Request.Method != null ? context.Request.Method.Method : null,
                Created = DateTime.Now
            };

            var resolver = GlobalConfiguration.Configuration.DependencyResolver;
            var service = (IExceptionLogService)resolver.GetService(typeof(IExceptionLogService));
            await service.Store(err);            
        }
    }
}