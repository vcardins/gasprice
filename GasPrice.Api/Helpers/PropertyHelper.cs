using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GasPrice.Api.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PropertyHelper<T>
    {
        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static PropertyInfo GetProperty<TValue>(Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
