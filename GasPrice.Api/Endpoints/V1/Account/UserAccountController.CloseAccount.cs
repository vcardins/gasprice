using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasPrice.Core.Extensions;
using GasPrice.Core.Account.Enum;
using GasPrice.Services.Account.Extensions;

namespace GasPrice.Api.Endpoints.V1.Account
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="GasPrice.Api.Endpoints.BaseApiController{GasPrice.Api.Hubs.MessagingHub}" />
    public partial class UserAccountController 
    {

        #region
        /**
         * @api {get} /api/users Get All 
         * @apiName GetAllUsers
         * @apiGroup User
         * @apiDescription      Retrieve a list of all users
         * @apiParam {string} include A list of children collections that must be returned along with the user profile
         * @apiParamExample {string} Request-Example:
         *       include=tasks,users 
         * @apiUse UserObject
         *
         */
        #endregion
        /// <summary>
        /// Posts the close account.
        /// </summary>
        /// <param name="confirm">if set to <c>true</c> [confirm].</param>
        /// <returns></returns>
        [Route("Close")]
        public HttpResponseMessage PostCloseAccount([FromBody] string confirm)
        {
            var response = Request.CreateResponse(HttpStatusCode.Continue);
            if (!bool.Parse(confirm))
                return response;
            
            _userService.DeleteAccount(User.GetUserID());
            
            response.Headers.Location = new Uri(_appInfo.LoginUrl);
            response.Headers.Add("message", UserAccountActionMessages.SuccessOnClosing.GetDescription());
            return response;
        }

    }
}
