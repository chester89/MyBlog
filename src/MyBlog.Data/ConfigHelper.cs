using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace MyBlog.Data
{
    public class NhConfigurationHelper
    {
        public static Configuration GetConfiguration()
        {
            var buildConfiguration = GetFluentConfiguration().BuildConfiguration();
            var schema = string.Empty;
            new SchemaExport(buildConfiguration).Execute(scr => { schema = scr; }, true, true);
            return buildConfiguration;
        }

        private static FluentConfiguration GetFluentConfiguration()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(builder => builder.FromConnectionStringWithKey("db")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BlogPostMap>()
                .Conventions.AddFromAssemblyOf<CustomIdConvention>());
        }

        public static ISessionFactory CreateSessionFactory()
        {
            var fluentConfiguration = GetFluentConfiguration();
            return fluentConfiguration.BuildSessionFactory();
        }
    }
}
