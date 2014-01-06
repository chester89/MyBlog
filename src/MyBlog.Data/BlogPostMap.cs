using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using MyBlog.Core.Entities;

namespace MyBlog.Data
{
    public class BlogMap: ClassMap<Blog>
    {
        public BlogMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            References(x => x.Author).Not.Nullable();
            HasMany(x => x.Posts);
        }
    }

    public class UserMap: ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable().Unique();
            Map(x => x.DisplayName).Length(100);
        }
    }

    public class BlogPostMap: ClassMap<BlogPost>
    {
        public BlogPostMap()
        {
            Id(x => x.Id);
            Map(x => x.Tags).Nullable();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Slug).Not.Nullable();
            Map(x => x.Text);
            References(x => x.Blog);
            Component(Reveal.Member<BlogPost, Zone>("timeZone"));
        }
    }

    public class ZoneMap : ComponentMap<Zone>
    {
        public ZoneMap()
        {
            Map(z => z.InstantTicks).Not.Nullable();
            Map(z => z.ZoneId).Not.Nullable().Length(100);
        }
    }
}
