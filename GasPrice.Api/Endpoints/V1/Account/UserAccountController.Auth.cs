using System.Web.Http;

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
        [Route("isauthenticated")]
        public IHttpActionResult GetIsAuthenticated()
        {
            return User.Identity.IsAuthenticated ? Ok() : Forbidden();
        }
    }
}
