#region credits
// ***********************************************************************
// Assembly	: Flext.Core
// Author	: Victor Cardins
// Created	: 03-19-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-21-2013
// ***********************************************************************
#endregion

using System.Collections.Generic;
using GasPrice.Core.Common.Validation;

namespace GasPrice.Services.Validation
{
    #region

    

    #endregion

    public class ValidationContainer<T> : IValidationContainer<T>
    {
        public IDictionary<string, IList<string>> Errors { get; private set; }
        public T Entity { get; private set; }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }

        public ValidationContainer(IDictionary<string, IList<string>> errors, T entity)
        {
            Errors = errors;
            Entity = entity;
        }
    }
}
