using System.Web.Http;
using GasPrice.Core.Account;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Common.Information;

namespace GasPrice.Api.Endpoints.V1.Account
{
    [RoutePrefix("api/account")]
    public partial class UserAccountController : BaseApiController 
    {
        private readonly IUserAccountService _userService;
        private readonly IApplicationInformation _appInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="appInfo">The application information.</param>
        public UserAccountController(IUserAccountService service, IApplicationInformation appInfo)
        {
            _userService = service;
            _appInfo = appInfo;

            Module = ModelType.UserAccount;
        }
    }
}
