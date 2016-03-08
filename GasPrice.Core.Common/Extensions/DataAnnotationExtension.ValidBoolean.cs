using System;
using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.Common.Extensions
{
    public partial class DataAnnotationExtension
    {
        public class ValidBooleanAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return value != null && Convert.ToBoolean(value);
            }
        }
    }
}
