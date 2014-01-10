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
    public class TagService: ITagService, IStartable
    {
        private readonly ISession session;
        private readonly ObjectCache cache;
        private const string tagsKey = "Tags";
        private const string tagsByPostKey = "TagsByPost";

        public TagService(ISession session, ObjectCache cache)
        {
            this.session = session;
            this.cache = cache;
        }

        private Dictionary<int, string[]> TagsByPost
        {
            get { return cache.Get(tagsByPostKey) as Dictionary<int, string[]>; }
        }

        TagModel[] AllTags
        {
            get { return cache.Get(tagsKey) as TagModel[]; }
        }

        string[] Split(string allTags)
        {
            return allTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        }

        public void UpdateTags(BlogPost newPost)
        {
            var newTags = Split(newPost.Tags);
            if (newTags.Any())
            {
                var counters = AllTags.ToList();
                foreach (var newTag in newTags)
                {
                    if (counters.Select(x => x.Name).Contains(newTag))
                    {
                        counters.SingleOrDefault(x => x.Name == newTag).PostCount += 1;
                    }
                    else
                    {
                        counters.Add(new TagModel()
                        {
                            Name = newTag, 
                            PostCount = 1
                        });
                    }
                }

                cache.Set(tagsKey, counters, DateTimeOffset.UtcNow.AddDays(100));
            }
        }

        public TagModel[] ByPost(Int32 postId)
        {
            var postTag = TagsByPost[postId];
            return AllTags.Where(t => postTag.Contains(t.Name)).ToArray();
        }

        public TagModel[] Cloud()
        {
            return AllTags;
        }

        public void Start()
        {
            var allPosts = session.Query<BlogPost>().AsEnumerable().Select(x => new
            {
                id = x.Id,
                tags = Split(x.Tags)
            }).ToList();

            var tagModel = allPosts.SelectMany(t => t.tags).GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count())
                .Select(x => new TagModel()
                {
                    Name = x.Key,
                    PostCount = x.Value
                }).ToArray();

            cache.Set(tagsKey, tagModel, DateTimeOffset.UtcNow.AddDays(100));

            cache.Set(tagsByPostKey, allPosts.ToDictionary(x => x.id, x => x.tags), DateTimeOffset.UtcNow.AddDays(50));
        }
    }
}
