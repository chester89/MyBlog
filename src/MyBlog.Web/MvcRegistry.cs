using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
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

            SetAllProperties(c =>
            {
                c.OfType<IActionInvoker>();
                //c.OfType<ITempDataProvider>();
                c.WithAnyTypeFromNamespaceContainingType<SlugToIdAttribute>();
            });
        }
    }
}
