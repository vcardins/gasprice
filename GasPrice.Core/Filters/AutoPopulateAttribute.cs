using System;

namespace GasPrice.Core.Filters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonIgnoreAttribute : Attribute
    {
    }
}