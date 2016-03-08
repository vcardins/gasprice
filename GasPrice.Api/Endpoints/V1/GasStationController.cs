using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Common.ViewModels;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.Filters;
using GasPrice.Core.Models.Modules;

namespace GasPrice.Api.Endpoints.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/gasstation")]
    [Module(Name = ModelType.Article)]
    public class GasStationController : CrudApiController<GasStation, GasStationOutputViewModel, GasStationInputViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GasStationController" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public GasStationController(IUnitOfWorkAsync unitOfWork) : base(unitOfWork)
        {
            ActionPolicies.Add(ModelAction.Create, UserRoleEnum.Admin);
            ActionPolicies.Add(ModelAction.Update, UserRoleEnum.Admin);
            ActionPolicies.Add(ModelAction.Delete, UserRoleEnum.Admin);
            ActionPolicies.Add(ModelAction.Export, UserRoleEnum.Admin);
            ActionPolicies.Add(ModelAction.Publish, UserRoleEnum.Admin);
        }

        protected override Expression<Func<GasStation, bool>> GetFilter(object id = null)
        {
            Expression<Func<GasStation, bool>> predicate = (n => true);
            if (id != null)
            {
                predicate = (n => n.Id == (int)id);
            }
            return predicate;
        }

        protected override Expression<Func<GasStation, bool>> GetExportFilter()
        {
            return (n => n.Enabled);
        }

        protected override Func<IQueryable<GasStation>, IOrderedQueryable<GasStation>> GetOrderBy()
        {
            return (q => q.OrderByDescending(x => x.Name));
        }

    }
}
