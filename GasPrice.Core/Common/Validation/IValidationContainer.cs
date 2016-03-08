using System.Collections.Generic;

namespace GasPrice.Core.Common.Validation
{
    public interface IValidationContainer<out T> : IValidationContainer
    {
        T Entity { get; }
    }

    public interface IValidationContainer
    {
        IDictionary<string, IList<string>> Errors { get; }

        bool IsValid { get; }
    }
}