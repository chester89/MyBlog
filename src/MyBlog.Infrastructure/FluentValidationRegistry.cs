using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using StructureMap.Configuration.DSL;

namespace MyBlog.Infrastructure
{
    public class FluentValidationRegistry : Registry
    {
        public FluentValidationRegistry()
        {
            Scan(sc =>
            {
                sc.TheCallingAssembly();
                sc.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
            });
        }
    }
}