using System.Web.Http;
using GasPrice.Core.Extensions;
using GasPrice.Api.Filters;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.ViewModels.UserAccount;
using GasPrice.Services.Account.Extensions;

namespace GasPrice.Api.Endpoints.V1.Account
{
    public partial class UserAccountController 
    {

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
        [Route("ChangeUsername")]
        [ModelStateValidator]
        public IHttpActionResult PostChangeUsername(ChangeUsernameInputModel model)
        {
            var userId = User.GetUserID();
            _userService.ChangeUsername(userId, model.NewUsername);
            //_authSvc.SignIn(User.GetUserID());

            return Ok(new { Message = UserAccountActionMessages.SuccessOnUpdating.GetDescription() });
        }

    }
}
