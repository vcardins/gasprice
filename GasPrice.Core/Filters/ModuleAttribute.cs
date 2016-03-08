using System;
using GasPrice.Core.Common.Enums;

namespace GasPrice.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        public ModelType Name { get; set; }

        public string Description { get; set; }
    }
}