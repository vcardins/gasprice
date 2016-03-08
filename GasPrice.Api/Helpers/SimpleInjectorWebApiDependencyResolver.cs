using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http.Dependencies;
using SimpleInjector;

namespace GasPrice.Api.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Http.Dependencies.IDependencyResolver" />
    public sealed class SimpleInjectorWebApiDependencyResolver : IDependencyResolver
    {
        private readonly Container _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorWebApiDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SimpleInjectorWebApiDependencyResolver(Container container)
        {
            _container = container;
        }

        [DebuggerStepThrough]
        public IDependencyScope BeginScope()
        {
            return this;
        }

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            return ((IServiceProvider)_container).GetService(serviceType);
        }

        [DebuggerStepThrough]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            IServiceProvider provider = _container;
            Type collectionType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var services = (IEnumerable<object>)provider.GetService(collectionType);
            return services ?? Enumerable.Empty<object>();
        }

        /// <summary>
        /// 
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
        }
    }
}