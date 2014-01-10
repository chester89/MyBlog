using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using MyBlog.Core;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;

namespace MyBlog.Data.Repositories
{
    public class CacheRepository: ICacheRepository
    {
        private readonly IRepository<BlogPost> posts;
        private readonly ObjectCache cache;
        private const int DefaultExpirationDayInterval = 50;

        public CacheRepository(IRepository<BlogPost> posts, ObjectCache cache)
        {
            this.posts = posts;
            this.cache = cache;
        }

        private const string tagsKey = "Tags";
        private const string tagsByPostKey = "TagsByPost";

        public void Init()
        {
            var allPosts = posts.GetAll().AsEnumerable().Select(x => new
            {
                id = x.Id,  
                tags = Tag.Split(x.Tags)
            }).ToList();

            var tagModel = allPosts.SelectMany(t => t.tags).GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count())
                .Select(x => new TagModel()
                {
                    Name = x.Key,
                    PostCount = x.Value
                }).ToArray();

            Tags = tagModel;

            TagsByPost = allPosts.ToDictionary(x => x.id, x => x.tags);
        }

        public TagModel[] Tags
        {
            get
            {
                return cache.Get(tagsKey) as TagModel[];
            }
            set
            {
                cache.Set(tagsKey, value, DateTimeOffset.UtcNow.AddDays(DefaultExpirationDayInterval));
            }
        }

        public IDictionary<Int32, String[]> TagsByPost
        {
            get
            {
                return cache.Get(tagsByPostKey) as Dictionary<Int32, String[]>;
            }
            set
            {
                cache.Set(tagsByPostKey, value, DateTimeOffset.UtcNow.AddDays(DefaultExpirationDayInterval));
            }
        }
    }
}
