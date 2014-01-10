using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace MyBlog.Infrastructure
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer container;

        public StructureMapDependencyResolver(Registry registry)
        {
            container = new Container(registry);
            container.Configure(x => x.Scan(sc =>
                {
                    sc.TheCallingAssembly();
                    sc.LookForRegistries();
                }));
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return container.TryGetInstance(serviceType);
            }
            return container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType).Cast<Object>();
        }

        public void DisposeOfHttpCachedObjects()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}