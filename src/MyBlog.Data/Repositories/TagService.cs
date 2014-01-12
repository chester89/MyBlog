using System;
using System.Collections.Generic;
using System.Linq;
using MyBlog.Core;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;

namespace MyBlog.Data.Repositories
{
    public class TagService: ITagService
    {
        private readonly ICacheRepository cache;

        public TagService(ICacheRepository cache)
        {
            this.cache = cache;
        }

        public void UpdateTags(BlogPost newPost)
        {
            var newTags = Tag.Split(newPost.Tags);
            if (newTags.Any())
            {
                var counters = cache.Tags.ToList();
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

                cache.Tags = counters.ToArray();
            }
        }

        public TagModel[] ByPost(Int32 postId)
        {
            var postTags = cache.TagsByPost[postId];
            return cache.Tags.Where(t => postTags.Contains(t.Name)).ToArray();
        }

        public TagModel[] Cloud()
        {
            return cache.Tags.OrderByDescending(x => x.PostCount).ThenBy(x => x.Name).ToArray();
        }
    }
}
