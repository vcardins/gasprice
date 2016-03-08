using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using GasPrice.Api.Filters;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Common;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Common.Extensions;
using GasPrice.Core.Common.Infrastructure;
using GasPrice.Core.Constants;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Services.Account.Extensions;
using Omu.ValueInjecter;

namespace GasPrice.Api.Endpoints
{

    public abstract class CrudApiController<TDomainModel> : CrudApiController<TDomainModel, TDomainModel, TDomainModel>
        where TDomainModel : class, IObjectState, new()
    {
        protected CrudApiController(IUnitOfWorkAsync unitOfWork) : base(unitOfWork) { }
    }

    public abstract class CrudApiController<TDomainModel, TOutputViewModel> : CrudApiController<TDomainModel, TOutputViewModel, TOutputViewModel>
        where TDomainModel : class, IObjectState, new()
        where TOutputViewModel : class, new()
    {
        protected CrudApiController(IUnitOfWorkAsync unitOfWork) : base(unitOfWork) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class CrudApiController<TDomainModel, TOutputViewModel, TInputViewModel> : BaseApiController
        where TDomainModel : class, IObjectState, new()
        where TOutputViewModel : class, new()
        where TInputViewModel : class, new()
    {

        protected readonly IUnitOfWorkAsync Uow;
        protected readonly IRepositoryAsync<TDomainModel> Repository;
        protected readonly IFtpClient Ftp;
        protected Dictionary<ModelAction, UserRoleEnum> ActionPolicies = new Dictionary<ModelAction, UserRoleEnum>();
        protected bool CollectionExportMode = true;

         /// <summary>
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        protected CrudApiController(IUnitOfWorkAsync unitOfWork)
        {
            Uow = unitOfWork;
            Repository = unitOfWork.RepositoryAsync<TDomainModel>();
            var resolver = GlobalConfiguration.Configuration.DependencyResolver;

            //_cache = resolver.GetService(typeof(IDataCache)) as IDataCache;
            Ftp = resolver.GetService(typeof(IFtpClient)) as IFtpClient;
        }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        protected abstract Expression<Func<TDomainModel, bool>> GetFilter(object id = null);

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <returns></returns>
        protected abstract Expression<Func<TDomainModel, bool>> GetExportFilter();

        protected virtual Task<IEnumerable<TOutputViewModel>> GetExportCollection()
        {
            return Task.FromResult<IEnumerable<TOutputViewModel>>(null);
        }

        protected virtual Task<TOutputViewModel> GetExportObject()
        {
            return Task.FromResult<TOutputViewModel>(null);
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <returns></returns>
        protected abstract Func<IQueryable<TDomainModel>, IOrderedQueryable<TDomainModel>> GetOrderBy();

        // Ts: aggregates the many little media lists in one payload
        // to reduce roundtrips when the client launches.
        // GET: api/medias
        /// <summary>
        /// Gets the medias.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] int page = 0, int size = 0)
        {
            var result = await GetRecords(page, size);
            AddHeader("X-TOTAL-RECORDS", result.Total);
            return Ok(result.Data);
        }

        protected virtual async Task<PackedList<TOutputViewModel>> GetRecords(int page = 0, int size = 0)
        {
            var query = Repository.Query(GetFilter()).OrderBy(GetOrderBy());
            var result = await query.SelectPagedAsync<TOutputViewModel>(page, size);
            return result;
        }

        #region
        /**
         * @api {get} /api/client/{id} Retrieve client by Id
         * @apiName GetById
         * @apiGroup Client
         * @apiDescription Returns the user indicated by the provided {id}
         * @apiVersion 2.0.0
         *
         * @apiParam {Int} id Client Id
         * @apiUse ClientObject
         */
        #endregion
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ActionName("GetById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var entity = await Repository.FirstOrDefaultAsync(GetFilter(id));
            if (entity == null) return NotFound(id);
            var model = new TOutputViewModel().InjectFrom(entity);
            return Ok(model);
        }

        #region
        /**
         * @api {post} /api/client Create Client
         * @apiName PostCreate
         * @apiGroup Client
         * @apiDescription Creates a new goal record
         * @apiVersion 2.0.0
         * @apiParam {object} model Client input model.
         * @apiParamExample {json} Request-Example:
         *      {  
         *          parentId : 15,
         *          managerId : 2,
         *          name : "Ryan Schmukler"      
         *      }
         * @apiSuccess {Object} data Client object
         * @apiSuccessExample Success-Response:
         *  HTTP/1.1 201 Created  
         */
        #endregion
        /// <summary>
        /// Posts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("")]
        [ModelStateValidator]
        [ActionName("Create")]
        [Log(Action = ModelAction.Create)]
        public async Task<IHttpActionResult> Post(TInputViewModel model)
        {
            UserRoleEnum role;
            if (ActionPolicies.TryGetValue(ModelAction.Create, out role))
            {
                User.CheckAccess(ClaimTypes.Role, new[] { role.ToString() });
            }

            var entity = new TDomainModel();
            entity.InjectFrom(new NullableInjection(new []{"Created"}), model);
            
            var props = entity.GetType().GetProperties();
            var tp = props.SingleOrDefault(x => x.Name == "CreatedById");
            tp?.SetValue(entity, CurrentUserId);

            await Repository.InsertAsync(entity, true);

            var result = new TOutputViewModel().InjectFrom<NullableInjection>(entity);

            return Ok(ModelAction.Create, EventStatus.Success, result);

        }

        /// <summary>
        /// Posts the specified model.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("slug")]
        [ActionName("Slug")]
        public IHttpActionResult GetSlug([FromUri] string title)
        {
            var slug = title.GenerateSlug();
            return Ok(slug);
        }

        /// <summary>
        /// Puts the specified model.
        /// </summary>
        /// <param name="id">The T identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [Log(Action = ModelAction.Update)]
        [ActionName("Update")]
        [ModelStateValidator]
        public async Task<IHttpActionResult> Put(int id, TInputViewModel model)
        {
            UserRoleEnum role;
            if (ActionPolicies.TryGetValue(ModelAction.Update, out role))
            {
                User.CheckAccess(ClaimTypes.Role, new[] { role.ToString() });
            }

            if (id <= 0) { return BadRequest(UserAccountConstants.InformationMessages.InvalidRequestParameters); }

            var entity = await Repository.FirstOrDefaultAsync(GetFilter(id));
            if (entity == null) { return NotFound(id); }


            entity.InjectFrom(new NullableInjection(new[] { "Id", "Created" }), model);
            
            var props = entity.GetType().GetProperties();
            var tp = props.SingleOrDefault(x => x.Name == "UpdatedById");
            tp?.SetValue(entity, CurrentUserId);

            await Repository.UpdateAsync(entity, true);

            var result = new TOutputViewModel().InjectFrom<NullableInjection>(entity);
            return Ok(ModelAction.Update, EventStatus.Success, result);
        }

        /// <summary>
        /// Deletes a T by its specified identifier.
        /// </summary>
        /// <param name="id">The T identifier.</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [Log(Action = ModelAction.Delete)]
        [ActionName("DeleteSingle")]
        public async Task<IHttpActionResult> DeleteSingle(int id)
        {
            UserRoleEnum role;
            if (ActionPolicies.TryGetValue(ModelAction.Delete, out role))
            {
                User.CheckAccess(ClaimTypes.Role, new[] { role.ToString() });
            }

            var success = await Repository.DeleteAsync(true, id);
            return Ok(ModelAction.Delete, success ? EventStatus.Success : EventStatus.Failure);
        }

        /// <summary>
        /// Deletes a T by its specified identifier.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        [Route("")]
        [ArrayInput("ids", typeof(int))]
        [Log(Action = ModelAction.Delete)]
        [ActionName("DeleteMany")]
        public async Task<IHttpActionResult> DeleteMany(List<int> ids)
        {
            UserRoleEnum role;
            if (ActionPolicies.TryGetValue(ModelAction.Delete, out role))
            {
                User.CheckAccess(ClaimTypes.Role, new[] { role.ToString() });
            }

            var sql = String.Format("DELETE FROM {0} WHERE Id IN ({1})", Module, String.Join(", ", ids.ToArray()));
            await Repository.ExecuteSql(sql);

            return Ok(ModelAction.Delete, EventStatus.Success);
        }

    }

}
