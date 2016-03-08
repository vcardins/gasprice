using System.Web.Http;
using GasPrice.Services.Account.Extensions;

namespace GasPrice.Api.Endpoints.V1.Account
{
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
        [Route("Signout")]
        public IHttpActionResult GetSignout()
        {
            var username = User.GetUserName();
            //authSvc.SignOut();                
            return Ok();
        }

    }
}
