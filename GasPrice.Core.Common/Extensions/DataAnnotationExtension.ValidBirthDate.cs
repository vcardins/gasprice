using System;
using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.Common.Extensions
{
    public partial class DataAnnotationExtension
    {
        public class ValidBirthDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return false;

                DateTime date;

                if(!DateTime.TryParse(value.ToString(), out date))
                    return false;

                if (date <= new DateTime(DateTime.Now.Year - 100, 1, 1))
                    return false;

                return true;
            }
        }
    }
}
