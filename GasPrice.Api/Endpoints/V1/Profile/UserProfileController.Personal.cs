using System.Web.Http;
using GasPrice.Core.Extensions;
using GasPrice.Api.Filters;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.ViewModels.UserProfile;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace GasPrice.Api.Endpoints.V1.Profile
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserProfileController
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
        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetProfile()
        {
            var profile = _userProfileService.GetProfile(CurrentUserId);
            return profile == null ? 
                   NotFound(UserAccountActionMessages.UserNotFound.GetDescription()) : 
                   Ok(profile);
        }

        /// <summary>
        /// Patches the profile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("")]
        [ModelStateValidator]
        public IHttpActionResult PatchProfile(UserProfileInfo model)
        {
            model.UserId = CurrentUserId;
            var entity = _userProfileService.Find(CurrentUserId);
            entity.InjectFrom(new LoopInjection(new[] { "PhotoId" }), model);

            _userProfileService.Update(entity);
            _userProfileService.Commit();

            var profile = new UserProfileInfo();
            profile.InjectFrom(entity);

            return Ok();
        }
       
    }
}
