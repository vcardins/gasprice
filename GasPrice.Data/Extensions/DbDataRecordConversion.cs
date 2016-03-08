#region credits
// ***********************************************************************
// Assembly	: GasPrice.Common
// Author	: Victor Cardins
// Created	: 03-23-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion
#region



#endregion

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;

// ReSharper disable CheckNamespace

namespace GasPrice.Data.Extensions
{
    #region

    

    #endregion

    public static class DbDataRecordConversion
    {
        /// <summary>
        ///   Converts a single DbDataRecord object into something else.
        ///   The destination type must have a default constructor.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "record"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this DbDataRecord record) where T : new()
        {
            var item = new T();
            for (int f = 0; f < record.FieldCount; f++)
            {
                var p = item.GetType().GetProperty(record.GetName(f));
                if (p != null && p.PropertyType == record.GetFieldType(f))
                {
                    p.SetValue(item, record.GetValue(f), null);
                }
            }

            return item;
        }

        /// <summary>
        ///   Converts a DbDataRecord ObjectQuery to a list of something else.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "query"></param>
        /// <returns></returns>
        public static List<T> ConvertTo<T>(this ObjectQuery<DbDataRecord> query) where T : new()
        {
            var list = query.ToList();

            return ConvertTo<T>(list);
        }

        /// <summary>
        ///   Converts a list of DbDataRecord to a list of something else.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "list"></param>
        /// <returns></returns>
        public static List<T> ConvertTo<T>(this List<DbDataRecord> list) where T : new()
        {
            var result = new List<T>();

            list.ForEach(rec => result.Add(rec.ConvertTo<T>()));

            return result;
        }
    }
}

