using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using GasPrice.Api.Extensions;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Constants;
using GasPrice.Core.Extensions;
using GasPrice.Core.Filters;
using GasPrice.Services.Account.Extensions;

namespace GasPrice.Api.Endpoints
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseApiController : ApiController 
        //ApiController
    {
       
        /// <summary>
        /// 
        /// </summary>
        protected string Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected int CurrentUserId => !User.Identity.IsAuthenticated ? 0 : User.GetUserId();

        private ModelType? _module;

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        protected ModelType Module
        {
            get
            {                
                if (_module.HasValue) return _module.GetValueOrDefault();
                _module = GetType().GetAttributeValue((ModuleAttribute dna) => dna.Name);
                return _module.GetValueOrDefault();
            }
            set { _module = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAdmin => User.Identity.IsAuthenticated && User.IsAdmin();

        /// <summary>
        /// Gets the current username.
        /// </summary>
        /// <value>
        /// The current username.
        /// </value>
        protected string CurrentUsername => !User.Identity.IsAuthenticated ? null : User.GetUserName();


        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddHeader(string key, object value )
        {
            HttpContext.Current.Response.Headers.Add(key, value.ToString());
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public void OnActionExecuting(HttpActionContext actionContext)
        {
            Controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
        }

        /// <summary>
        /// Oks the specified event action.
        /// </summary>
        /// <param name="eventAction">The event action.</param>
        /// <param name="eventStatus">The event status.</param>
        /// <param name="data">The data.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        protected IHttpActionResult Ok(ModelAction eventAction, EventStatus eventStatus, object data = null, string msg = null)
        {
            var action = eventAction.GetDescription().ToLower();
            if (string.IsNullOrEmpty(msg)) {
                switch (eventStatus)
                {
                    case EventStatus.Success:
                        msg = String.Format("{0} {1} com sucesso", string.Format("{0}(s)",Module.GetDescription()), action);
                        break;
                    case EventStatus.Failure:
                        msg = String.Format("Um erro ocorreu. {0} não pode ser {1}", Module.GetDescription(), action);
                        break;
                    case EventStatus.Pending:
                        msg = String.Format("{0} ação {1} está pendente no momento", action, Module.GetDescription());
                       break;
                    case EventStatus.NoAction:
                       msg = String.Format("{0} está atualizado(a). Nenhuma ação foi realizada", Module.GetDescription());
                        break;
                }
            }
            var content = data == null ? (object) new {Message = msg} : new {Message = msg, Data = data};
            return Ok(content);
        }

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        protected IHttpActionResult InvalidModel()
        {
            return BadRequest(UserAccountConstants.InformationMessages.InvalidRequestParameters);
        }

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        protected IHttpActionResult NotFound(string message = null)
        {
            var msg = message ?? String.Format(UserAccountConstants.InformationMessages.NotFound, Module.GetDescription());
            return new NotFoundWithMessageResult(msg);   
        }

        /// <summary>
        /// Forbiddens the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected IHttpActionResult Forbidden(string message = null)
        {
            var msg = message ?? String.Format(UserAccountConstants.InformationMessages.AccessNotAllowed);
            return new ForbiddenWithMessageResult(msg);   
        }

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        protected IHttpActionResult NotFound(int id)
        {
            return NotFound(String.Format(UserAccountConstants.InformationMessages.NotFound2, Module.GetDescription(), id));
        }

        /// <summary>
        /// Bads the request.
        /// </summary>
        /// <param name="eventAction">The event action.</param>
        /// <returns></returns>
        protected BadRequestResult BadRequest(ModelAction eventAction)
        {
            return BadRequest();
        }
    }
}
