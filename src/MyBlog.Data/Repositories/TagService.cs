using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;
using NHibernate;
using NHibernate.Linq;

namespace MyBlog.Data.Repositories
{
    public class TagService: ITagService
    {
        private readonly ISession _session;
        private readonly ObjectCache cache;
        private string tagsKey = "Tags";

        public TagService(ISession session, ObjectCache cache)
        {
            _session = session;
            this.cache = cache;
        }

        public void CalculateTagCounters()
        {
            var allPosts = _session.Query<BlogPost>().AsEnumerable().Select(x => new
            {
                id = x.Id, 
                title = x.Title, 
                text = x.Text,
                created = x.Created,
                slug = x.Slug,
                blog = x.Blog.Name,
                blogAuthor = x.Blog.Author.DisplayName,
                tags = x.Tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            }).ToList();

            var tagModel = allPosts.SelectMany(t => t.tags).GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count())
                .Select(x => new TagModel()
            {
                Name = x.Key,
                PostCount = x.Value
            }).ToArray();

            cache.Set(tagsKey, tagModel, DateTimeOffset.UtcNow.AddDays(100));
        }

        string[] TagsByPost(Int32 postId)
        {
            return _session.Query<BlogPost>().SingleOrDefault(p => p.Id == postId).Tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public TagModel[] ByPost(Int32 postId)
        {
            var tagCounters = cache.Get(tagsKey) as TagModel[];
            var postTag = TagsByPost(postId);
            return tagCounters.Where(t => postTag.Contains(t.Name)).ToArray();
        }

        public TagModel[] Cloud()
        {
            return new TagModel[];
        }
    }
}
