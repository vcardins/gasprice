using System;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace GasPrice.Api.Filters
{
    public class DateTimeModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            ValidateBindingContext(bindingContext);

            if (!bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName) ||
                !CanBindType(bindingContext.ModelType))
            {
                return false;
            }

            bindingContext.Model = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName)
                .ConvertTo(bindingContext.ModelType, Thread.CurrentThread.CurrentCulture);

            bindingContext.ValidationNode.ValidateAllProperties = true;

            return true;
        }

        private static void ValidateBindingContext(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            if (bindingContext.ModelMetadata == null)
            {
                throw new ArgumentException("ModelMetadata cannot be null", "bindingContext");
            }
        }

        public static bool CanBindType(Type modelType)
        {
            return modelType == typeof(DateTime) || modelType == typeof(DateTime?);
        }
    }
}