using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Data.Repositories;
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
            container.Configure(x => x.AddRegistry<NhRegistry>());
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
            return container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }

        public void DisposeOfHttpCachedObjects()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}