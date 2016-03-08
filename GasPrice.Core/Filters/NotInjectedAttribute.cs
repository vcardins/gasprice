using System;

namespace GasPrice.Core.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotInjectedAttribute : Attribute
    {
    }
}