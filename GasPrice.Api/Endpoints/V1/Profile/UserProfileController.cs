using System.Web.Http;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Services;

namespace GasPrice.Api.Endpoints.V1.Profile
{

    [RoutePrefix("api/profile")]
    public partial class UserProfileController : BaseApiController 
    {

        private readonly IUserProfileService _userProfileService;
        /**
        * @apiDefine UserAccount
        * @apiSuccess {Object} data User object
        * @apiSuccessExample Success-Response:
            {
                "data": {
                    "firstname": "Victor",
                    "lastname": "Cardins",
                    "name": "Victor Cardins",
                    "email": "victor@results.com",
                    "mobilePhoneNumber": "6043665378",
                    "password": "",
                    "confirmPassword": "",
                }
            } 
       */

        /**
        * @apiDefine UserNotFoundError
        * @apiError UserNotFound The id of the User was not found.
        * @apiErrorExample Error-Response:
        *     HTTP/1.1 404 Not Found
        *     {
        *       "error": "UserNotFound"
        *     }
        */


        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileController"/> class.
        /// </summary>
        /// <param name="userProfileService">The user profile service.</param>
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            Module = ModelType.UserAccount;
        }
    }
}
