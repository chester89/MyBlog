using System;
using System.Collections.Generic;
using System.Linq;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace MyBlog.Data.Repositories
{
    public class TagCounter: ITagCounter
    {
        private readonly ISession _session;

        public TagCounter(ISession session)
        {
            _session = session;
        }

        public IDictionary<String, Int32> ByPost(Int32 postId)
        {
            var allTags = _session.Query<BlogPost>().Select(x => new { id = x.Id, tags = x.Tags.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries) }).ToList();
            return allTags.SelectMany(t => t.tags).GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count())
                .Where(pair => allTags.Single(t => t.id == postId).tags.Contains(pair.Key)).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
