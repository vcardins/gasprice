using System;
using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BooleanRequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (bool)value == true;
        }
    }
}