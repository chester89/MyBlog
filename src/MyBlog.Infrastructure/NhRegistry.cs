using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Data;
using MyBlog.Data.Repositories;
using NHibernate;
using StructureMap.Configuration.DSL;

namespace MyBlog.Infrastructure
{
    public class NhRegistry : Registry
    {
        public NhRegistry()
        {
            For<ISessionFactory>().Singleton().Use(NhConfigurationHelper.CreateSessionFactory);
            For<ISession>().HttpContextScoped().Use(context => context.TryGetInstance<ISessionFactory>().OpenSession());
            For(typeof (IRepository<>)).Use(typeof (NhRepositoryBase<>));
            For<IDbConnection>().HttpContextScoped()
                .Use(context => new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString));
            Scan(sc =>
            {
                sc.AssemblyContainingType<BlogPost>();
                sc.AssemblyContainingType<SlugService>();
                sc.RegisterConcreteTypesAgainstTheFirstInterface();
                sc.WithDefaultConventions();
                sc.TheCallingAssembly();
            });
        }
    }
}
