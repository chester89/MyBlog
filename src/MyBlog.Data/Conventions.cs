using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace MyBlog.Data
{
    public class CustomIdConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
        }
    }

    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        protected override String GetKeyName(Member property, Type type)
        {
            return property == null ? type.Name + "Id" : property.Name + "Id";
        }
    }

    public class CustomCascadeConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.SaveUpdate();
            //instance.LazyLoad();
        }
    }

    public class CustomMany2ManyCascadeConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Cascade.All();
            //instance.LazyLoad();
        }
    }

    public class CustomManyToManyConvention : ManyToManyTableNameConvention
    {
        private const string Delimiter = "To";

        protected override String GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
        {
            return otherSide.EntityType.Name + Delimiter + collection.EntityType.Name;
        }

        protected override String GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
        {
            return collection.EntityType.Name + Delimiter + "Related";
        }
    }
}
