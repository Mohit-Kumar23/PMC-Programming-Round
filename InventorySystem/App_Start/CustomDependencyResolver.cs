using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace InventorySystem
{
    internal class CustomDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IDependencyScope BeginScope()
        {
            return new CustomDependencyResolver(_serviceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose()
        {
            // do nothing
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, serviceType);
            }
            catch
            {
                return null; // Return null if the service is not registered
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }
    }
}