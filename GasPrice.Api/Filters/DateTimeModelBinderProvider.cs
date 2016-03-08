using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace GasPrice.Api.Filters
{
    public class DateTimeModelBinderProvider : ModelBinderProvider
    {
        readonly DateTimeModelBinder _binder = new DateTimeModelBinder();

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if (DateTimeModelBinder.CanBindType(modelType))
            {
                return _binder;
            }

            return null;
        }
    }
}