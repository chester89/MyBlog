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
        private readonly IRepository<BlogPost> postRepository;
        private readonly ObjectCache cache;
        private const int DefaultExpirationDayInterval = 50;

        public CacheRepository(IRepository<BlogPost> postRepository, ObjectCache cache)
        {
            this.postRepository = postRepository;
            this.cache = cache;
        }

        private const string tagsKey = "Tags";
        private const string tagsByPostKey = "TagsByPost";
        private const string allPostsKey = "AllPosts";

        public void Init()
        {
            var allPosts = postRepository.GetAll().AsEnumerable().Select(x => new
            {
                id = x.Id,  
                post = x,
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
            Posts = allPosts.Select(p => p.post).ToList();
        }

        public TagModel[] Tags
        {
            get { return cache.Get(tagsKey) as TagModel[]; }
            set { cache.Set(tagsKey, value, DateTimeOffset.UtcNow.AddDays(DefaultExpirationDayInterval)); }
        }

        public IDictionary<Int32, String[]> TagsByPost
        {
            get { return cache.Get(tagsByPostKey) as Dictionary<Int32, String[]>; }
            set { cache.Set(tagsByPostKey, value, DateTimeOffset.UtcNow.AddDays(DefaultExpirationDayInterval)); }
        }

        public IEnumerable<BlogPost> Posts
        {
            get { return cache.Get(allPostsKey) as List<BlogPost>; }
            set { cache.Set(allPostsKey, value, DateTimeOffset.UtcNow.AddDays(DefaultExpirationDayInterval)); }
        }

        public void AddNewPost(BlogPost newPost)
        {
            var posts = Posts.ToList();
            posts.Add(newPost);
            Posts = posts;
        }
    }
}
