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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

// ReSharper disable CheckNamespace

namespace GasPrice.Data.Extensions
{
    #region

    

    #endregion

    public static class ContextExtensions
    {
        //===============================================================
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            var adapter = (IObjectContextAdapter)context;
            var objectContext = adapter.ObjectContext;
            return objectContext.GetTableName<T>();
        }

        public static PrimitivePropertyConfiguration HasUniqueIndexAnnotation(
            this PrimitivePropertyConfiguration property,
            string indexName,
            int columnOrder,
            bool isUnique)
        {
            var indexAttribute = new IndexAttribute(indexName, columnOrder) { IsUnique = isUnique };
            var indexAnnotation = new IndexAnnotation(indexAttribute);

            return property.HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
        }

        //===============================================================
        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
        //===============================================================
        public static void BulkAdd<T>(this DbContext context, IEnumerable<T> items) where T : class
        {
            var tableName = GetTableName<T>(context);
            var dataTable = items.ToDataTable();
            var connection = (SqlConnection)context.Database.Connection;

            var isClosed = connection.State != ConnectionState.Open;
            if (isClosed)
                connection.Open();

            var bulkCopy = new SqlBulkCopy(connection) {DestinationTableName = tableName};
            bulkCopy.WriteToServer(dataTable);

            if (isClosed)
                connection.Close();
        }
        //===============================================================
        public static Func<TValue, Object[]> GetKeySelector<TValue>(this DbContext context) where TValue : class
        {
            var keyNames = context.GetKeyNames<TValue>();
            return x => keyNames.Select(name => typeof(TValue).GetProperty(name).GetValue(x)).ToArray();
        }
        //===============================================================
        public static IEnumerable<String> GetKeyNames<TValue>(this DbContext context) where TValue : class
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var set = objectContext.CreateObjectSet<TValue>();
            var keyNames = set.EntitySet.ElementType.KeyMembers.Select(x => x.Name).ToList();

            return keyNames;
        }
        //===============================================================

        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
    }
}

