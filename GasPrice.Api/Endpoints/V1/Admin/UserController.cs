using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GasPrice.Core.Services;

namespace GasPrice.Api.Endpoints.V1.Admin
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="GasPrice.Api.Endpoints.BaseApiController{GasPrice.Api.Hubs.MessagingHub}" />
    [RoutePrefix("api/users")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetAll([FromUri] int pageIndex = 1, [FromUri] int pageSize = 10)
        {
            //int total = 0;
            var list = _service.GetUsers();
            //AddHeader("X-TOTAL-RECORDS", total);
            return Ok(list);
        }

        #region
        /**
         * @api {get} /api/notificationType/{id} Retrieve notificationType by Id
         * @apiName GetById
         * @apiGroup User
         * @apiDescription Returns the user indicated by the provided {id}
         * @apiVersion 2.0.0
         *
         * @apiParam {Int} id User Id
         * @apiUse UserObject
         */
        #endregion
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var record = await _service.FindAsync(id);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        // /api/persons/getbyfirstname?value=Joe1
        /// <summary>
        /// Gets the first name of the by.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [ActionName("getbyfirstname")]
        public IHttpActionResult GetByFirstName(string value)
        {
            var record = _service.Queryable().FirstOrDefault(p => p.FirstName.StartsWith(value));
            if (record == null)
                return NotFound();

            return Ok(record);
        }

    }
}
