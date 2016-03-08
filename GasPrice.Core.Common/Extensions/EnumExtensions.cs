#region credits
// ***********************************************************************
// Assembly	: GasPrice.Common
// Author	: Momentum
// Created	: 03-23-2013
// 
// Last Modified By : Momentum
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion
#region



#endregion

// ReSharper disable CheckNamespace
using System;
using System.ComponentModel;

namespace GasPrice.Core.Extensions
{
    #region

    

    #endregion

    public static class EnumExtensions
        // ReSharper restore CheckNamespace
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            return field.Name;
        }
    }
}

