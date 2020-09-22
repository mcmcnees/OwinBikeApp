using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;

namespace BikeMgr.API
{
    public sealed class UnityDependencyResolver : IDependencyResolver
    {
        private IUnityContainer container;
        private SharedDependencyScope sharedScope;

        public UnityDependencyResolver(IUnityContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
            this.sharedScope = new SharedDependencyScope(container);
        }

        public IDependencyScope BeginScope()
        {
            return this.sharedScope;
        }

        public void Dispose()
        {
            this.container.Dispose();
            this.sharedScope.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return this.container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this.container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        private sealed class SharedDependencyScope : IDependencyScope
        {
            private IUnityContainer container;

            public SharedDependencyScope(IUnityContainer container)
            {
                this.container = container;
            }

            public object GetService(Type serviceType)
            {
                return this.container.Resolve(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return this.container.ResolveAll(serviceType);
            }

            public void Dispose()
            {
                
            }
        }
    }
}