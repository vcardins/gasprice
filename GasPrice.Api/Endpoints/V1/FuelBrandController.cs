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
    [RoutePrefix("api/fuelbrand")]
    [Module(Name = ModelType.FuelBrand)]
    public class FuelBrandController : CrudApiController<FuelBrand, FuelBrandViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FuelBrandController" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public FuelBrandController(IUnitOfWorkAsync unitOfWork) : base(unitOfWork)
        {
            ActionPolicies.Add(ModelAction.Create, UserRoleEnum.Admin);
            ActionPolicies.Add(ModelAction.Update, UserRoleEnum.Admin);
            ActionPolicies.Add(ModelAction.Delete, UserRoleEnum.Admin);
            ActionPolicies.Add(ModelAction.Export, UserRoleEnum.None);
            ActionPolicies.Add(ModelAction.Publish, UserRoleEnum.None);
        }

        protected override Expression<Func<FuelBrand, bool>> GetFilter(object id = null)
        {
            Expression<Func<FuelBrand, bool>> predicate = (n => true);
            if (id != null)
            {
                predicate = (n => n.Id == (int) id);
            }
            return predicate;
        }

        protected override Expression<Func<FuelBrand, bool>> GetExportFilter()
        {
            Expression<Func<FuelBrand, bool>> predicate = (n => true);
            return predicate;
        }

        protected override Func<IQueryable<FuelBrand>, IOrderedQueryable<FuelBrand>> GetOrderBy()
        {
            return (q => q.OrderBy(x => x.SortOrder));
        }

    }
}
