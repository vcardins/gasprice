using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GasPrice.Api.Filters;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Models;
using GasPrice.Core.Services;
using GasPrice.Core.ViewModels;

namespace GasPrice.Api.Endpoints.V1.Admin
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MessagingHub" />
    [RoutePrefix("api/admin/settings")]
    [ClaimsAuthorize("Roles", "Admin")]
    public class AppSettingsController : BaseApiController
    {
        private readonly IAppSettingsService _appSettingsService;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsController"/> class.
        /// </summary>
        public AppSettingsController(IAppSettingsService appSettingsService)
        {
            Module = ModelType.AppSettings;
            _appSettingsService = appSettingsService;
        }

        // GET: api/appSettingss
        /// <summary>
        /// Gets the FAQs.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<AppSettings>))]
        public async Task<IHttpActionResult> Get()
        {
            var settings = await _appSettingsService.GetAppSettings();
            AddHeader("X-TOTAL-RECORDS", 1);
            return Ok(settings);
        }
   
        #region
        /* ADMIN Actions */

        /// <summary>
        /// Posts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [ClaimsAuthorize(ClaimTypes.Role, "Admin")]
        [Route("")]
        [ModelStateValidator]
        public async Task<IHttpActionResult> Post(AppSettingsInput model)
        {
            return await AddOrUpdate(model);
        }

        private async Task<IHttpActionResult> AddOrUpdate(AppSettingsInput model, IDictionary<string, object> fields = null)
        {
            if (model == null && fields == null) { return BadRequest(); }
            var action = await _appSettingsService.SaveSettings(model);
            return Ok(action, EventStatus.Success); 
            
        }

        /// <summary>
        /// Puts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [ClaimsAuthorize(ClaimTypes.Role, "Admin")]
        [Route("")]
        [ModelStateValidator]
        public async Task<IHttpActionResult> Put(AppSettingsInput model)
        {
            return await AddOrUpdate(model);      
        }

        /// <summary>
        /// Patches the specified identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [ClaimsAuthorize(ClaimTypes.Role, "Admin")]
        [Route("")]
        public async Task<IHttpActionResult> Patch(IDictionary<string, object> model)
        {
            return await AddOrUpdate(null, model);            
        }

         #endregion
    }
}
