using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;
using MyBlog.Data;
using StructureMap.Configuration.DSL;

namespace MyBlog.Infrastructure
{
    public class ServiceRegistry: Registry
    {
        public ServiceRegistry()
        {
            Scan(x =>
                {
                    x.AssemblyContainingType<ISlugService>();
                    x.AssemblyContainingType<SlugService>();
                    x.AddAllTypesOf<IStartable>();
                });
        }
    }
}
