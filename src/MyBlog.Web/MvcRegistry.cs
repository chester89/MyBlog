using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web.Mvc;
using MyBlog.Core.Contracts;
using MyBlog.Infrastructure;
using MyBlog.Web.Attributes;
using StructureMap.Configuration.DSL;

namespace MyBlog.Web
{
    public class MvcRegistry: Registry
    {
        public MvcRegistry()
        {
            For<IActionInvoker>().Use<InjectingActionInvoker>();
            For<ObjectCache>().Singleton().Use(c => MemoryCache.Default);

            SetAllProperties(c =>
            {
                c.OfType<IActionInvoker>();
                c.WithAnyTypeFromNamespaceContainingType<ISlugService>();
                c.WithAnyTypeFromNamespaceContainingType<SlugToIdAttribute>();
            });
        }
    }
}
