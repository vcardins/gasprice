using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GasPrice.Core.Extensions;
using GasPrice.Api.Filters;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Common.Extensions;
using GasPrice.Core.ViewModels.UserAccount;
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
        /// <summary>
        /// Gets the specified includes.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        [Route("")]
        [ArrayInput("includes", typeof(string))]
        public async Task<IHttpActionResult> Get([FromUri] string includes)
        {
            var userId = User.GetUserId();
            var record = await _userService.FindAsync(userId);
            return Ok(record);
        }

        /// <summary>
        /// Gets the is username available.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [Route("IsUsernameAvailable")]
        [AllowAnonymous]
        public IHttpActionResult GetIsUsernameAvailable([FromUri]string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return Ok(new { Success = false, Message = "Username is required", Data = username });
            }

            var isAvailable = !_userService.UsernameExists(username);
            return Ok(new
            {
                Success = isAvailable, 
                Message = isAvailable ? "Username is available" : "Sorry, the username you selected is already taken"
            });
        }

        #region
        /**
         * @api {get} /api/account/isemailavailable Check if email is available
         * @apiName GetIsEmailAvailable
         * @apiGroup Account
         * @apiDescription Check if email address is available
         *
         * @apiParam {String} email Email
         */
        #endregion
        /// <summary>
        /// Gets the is email available.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [Route("IsEmailAvailable")]
        [AllowAnonymous]
        public IHttpActionResult GetIsEmailAvailable([FromUri]string email)
        {
            if (!email.IsValidEmail())
            {
                return BadRequest("Email is invalid");
            }

            var isAvailable = !_userService.EmailExists(email);
            return Ok(new
            {
                Success = isAvailable, 
                Message = isAvailable ? "Email is available" : "Sorry, the email you selected is already taken"
            });
        }

        #region
        /**
         * @api {post} /api/users Create New User
         * @apiName CreateUser
         * @apiGroup User
         * @apiDescription   Creates a new user record
         * @apiVersion 2.0.0
         * @apiParam {object} model User input model.
         * @apiParamExample {json} Request-Example:
         *       {
         *          UserPermissionsMap: null,
                    address1: "1600D Beach Ave",
                    address2: "Suite #1208",
                    avatarUrl: "https://resultsprod.blob.core.windows.net/images-profile/user-301789-1d8235223d3f403fb3dc006c233ba0bc.jpg"
                    city: "Vancouver",
                    context: {},
                    country_id: null,
                    email: "victor@results.com",
                    firstname: "Victor",
                    id: 301789,
                    lastname: "Cardins",
                    member_of_business_unit_ids: null,
                    name: "Victor Cardins",
                    phone: "6043665378",
                    photo_url: "https://resultsprod.blob.core.windows.net/images-profile/user-301789-1d8235223d3f403fb3dc006c233ba0bc.jpg"
                    postalCode: "V6G1Y8",
                    postalcode: "",
                    preferences: {
                          defaultTimeZoneId: "Pacific Standard Time",
                          dateFormat: "mm/dd/yy", 
                          dailyEmailSummaryEnabled: true
                    }
                    roleId: 118093,
                    role_id: 118093,
                    role_name: null,
                    state: "BC",
                    notificationTypeId: 0,
                    timezone_id: null,
                    title: "Site Admin"
         *      } 
         * @apiSuccess {object} user User user
         * @apiSuccessExample Success-Response:
         *     HTTP/1.1 200 OK 
         */
        #endregion
        /// <summary>
        /// Posts the register user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("register")]
        [AllowAnonymous]
        [ModelStateValidator]
        public IHttpActionResult PostRegisterUser(RegisterInputModel model)
        {
            _userService.CreateAccount(model);
            return Ok(new { Message = UserAccountActionMessages.SuccessOnCreatingEmailValidation.GetDescription() });
        }
        //[ClaimsAuthorize(ClaimTypes.Role, "Admin")]
        /// <summary>
        /// Posts the verify email.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [AllowAnonymous]
        //[ModelStateValidator]
        [Route("verify/{key}")] //ChangeEmailFromKeyInputModel model
        public HttpResponseMessage GetVerifyEmail([FromUri] string key)
        {
            _userService.VerifyEmailFromKey(key);
            
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(_appInfo.HomePage + _appInfo.LoginUrl);
            response.Headers.Add("message", UserAccountActionMessages.SuccessOnVerifying.GetDescription());
            return response;
        }

        /// <summary>
        /// Posts the cancel email verification.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("verify/cancel/{key}")]
        public HttpResponseMessage PostCancelEmailVerification(string key)
        {
            _userService.CancelVerification(key);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Location = new Uri(_appInfo.HomePage + _appInfo.LoginUrl);
            response.Headers.Add("message", UserAccountActionMessages.SuccessOnCanceling.GetDescription());
            return response;
        }

    }
}
