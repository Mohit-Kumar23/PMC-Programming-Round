using InventorySystem.Interfaces;
using InventorySystem.Repository;
using InventorySystem.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace InventorySystem
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IServiceProvider serviceProvider { get; private set; }
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Configure Dependency Injection
            var services = new ServiceCollection();

            // Register your services here
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IInventoryService, InventoryService>();

            // Build Service Provider
            serviceProvider = services.BuildServiceProvider();

            // Set Dependency Resolver
            GlobalConfiguration.Configuration.DependencyResolver = new CustomDependencyResolver(serviceProvider);

        }
    }
}
