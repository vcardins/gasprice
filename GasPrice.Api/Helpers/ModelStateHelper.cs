using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace GasPrice.Api.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelStateHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string[]>> Errors(this ModelStateDictionary modelState)
        {
            if (modelState.IsValid) return null;

            var errors = modelState.ToDictionary(kvp => kvp.Key.Replace("model.",""), kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                .Where(m => m.Value.Any());

            return errors;
        }
    }
}
