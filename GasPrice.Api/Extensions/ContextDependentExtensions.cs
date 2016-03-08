using System;
using System.Diagnostics;
using System.Linq.Expressions;
using SimpleInjector;

namespace GasPrice.Api.Extensions
{

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay(
        "DependencyContext (ServiceType: {ServiceType}, " +
        "ImplementationType: {ImplementationType})")]
    public class DependencyContext
    {
        internal static readonly DependencyContext Root =
            new DependencyContext();

        internal DependencyContext(Type serviceType,
            Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }

        private DependencyContext()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ImplementationType { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ContextDependentExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="contextBasedFactory"></param>
        /// <typeparam name="TService"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RegisterWithContext<TService>(
            this Container container,
            Func<DependencyContext, TService> contextBasedFactory)
            where TService : class
        {
            if (contextBasedFactory == null)
            {
                throw new ArgumentNullException("contextBasedFactory");
            }

            Func<TService> rootFactory =
                () => contextBasedFactory(DependencyContext.Root);

            container.Register(rootFactory, Lifestyle.Scoped);

            // Allow the Func<DependencyContext, TService> to be 
            // injected into parent types.
            container.ExpressionBuilding += (sender, e) =>
            {
                if (e.RegisteredServiceType != typeof (TService))
                {
                    var rewriter = new DependencyContextRewriter
                    {
                        ServiceType = e.RegisteredServiceType,
                        ContextBasedFactory = contextBasedFactory,
                        RootFactory = rootFactory,
                        Expression = e.Expression
                    };

                    e.Expression = rewriter.Visit(e.Expression);
                }
            };
        }

        private sealed class DependencyContextRewriter : ExpressionVisitor
        {
            internal Type ServiceType { get; set; }

            internal object ContextBasedFactory { get; set; }

            internal object RootFactory { get; set; }

            internal Expression Expression { get; set; }

            internal Type ImplementationType
            {
                get
                {
                    var expression = Expression as NewExpression;

                    if (expression != null)
                    {
                        return expression.Constructor.DeclaringType;
                    }

                    return ServiceType;
                }
            }

            protected override Expression VisitInvocation(
                InvocationExpression node)
            {
                if (!IsRootedContextBasedFactory(node))
                {
                    return base.VisitInvocation(node);
                }

                return Expression.Invoke(
                    Expression.Constant(ContextBasedFactory),
                    Expression.Constant(
                        new DependencyContext(
                            ServiceType,
                            ImplementationType)));
            }

            private bool IsRootedContextBasedFactory(
                InvocationExpression node)
            {
                var expression =
                    node.Expression as ConstantExpression;

                if (expression == null)
                {
                    return false;
                }

                return ReferenceEquals(expression.Value,
                    RootFactory);
            }
        }
    }
}