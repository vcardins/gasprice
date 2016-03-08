using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace GasPrice.Core.EventHandling
{
    class GenericMethodActionBuilder<TTargetBase, TParamBase>
    {
        readonly ConcurrentDictionary<Type, Action<TTargetBase, TParamBase>> _actionCache = new ConcurrentDictionary<Type, Action<TTargetBase, TParamBase>>();

        readonly Type _targetType;
        readonly string _method;
        public GenericMethodActionBuilder(Type targetType, string method)
        {
            _targetType = targetType;
            _method = method;
        }

        public Action<TTargetBase, TParamBase> GetAction(TParamBase paramInstance)
        {
            var paramType = paramInstance.GetType();

            if (!_actionCache.ContainsKey(paramType))
            {
                _actionCache[paramType] = BuildActionForMethod(paramType);
            }

            return _actionCache[paramType];
        }

        private Action<TTargetBase, TParamBase> BuildActionForMethod(Type paramType)
        {
            var handlerType = _targetType.MakeGenericType(paramType);

            var ehParam = Expression.Parameter(typeof(TTargetBase));
            var evtParam = Expression.Parameter(typeof(TParamBase));
            var invocationExpression =
                Expression.Lambda(
                    Expression.Block(
                        Expression.Call(
                            Expression.Convert(ehParam, handlerType),
                            handlerType.GetMethod(_method),
                            Expression.Convert(evtParam, paramType))),
                    ehParam, evtParam);

            return (Action<TTargetBase, TParamBase>)invocationExpression.Compile();
        }
    }
}
