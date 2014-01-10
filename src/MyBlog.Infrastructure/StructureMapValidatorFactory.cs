using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace MyBlog.Infrastructure
{
    public class StructureMapValidatorFactory : ValidatorFactoryBase
    {
        private readonly IContainer container;

        public StructureMapValidatorFactory(Registry registry)
        {
            container = new Container();
            container.Configure(c => c.AddRegistry(registry));
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return container.TryGetInstance(validatorType) as IValidator;
        }
    }
}