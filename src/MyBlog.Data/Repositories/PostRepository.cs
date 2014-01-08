using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;
using NHibernate;
using NHibernate.Linq;

namespace MyBlog.Data.Repositories
{
    public class PostRepository : NhRepositoryBase<BlogPost>, IPostRepository
    {
        private readonly IShortenAlgorithm shorten;

        public PostRepository(ISession session, IShortenAlgorithm shorten) : base(session)
        {
            this.shorten = shorten;
        }

        public void AddNew(BlogPost newPost, int blogId)
        {
            var blog = Session.Get<Blog>(blogId);
            newPost.Blog = blog;
            Session.SaveOrUpdate(newPost);
        }

        public PostReadModel Read(int postId)
        {
            var allPosts = Session.Query<BlogPost>().AsEnumerable().Select(x => new
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
                .Where(pair => allPosts.Single(t => t.id == postId).tags.Contains(pair.Key)).Select(x => new TagModel()
                    {
                        Name = x.Key,
                        PostCount = x.Value
                    }).ToArray();
            var post = allPosts.Single(x => x.id == postId);
            return new PostReadModel()
            {
                Id = post.id,
                Title = post.title,
                Text = post.text,
                Slug = post.slug,
                Created = post.created.ToDateTimeUtc(),
                Tags = tagModel,
                BlogName = post.blog,
                BlogAuthorDisplayName = post.blogAuthor
            };
        }

        public IEnumerable<PostReadModel> List()
        {
            var allPosts = Session.Query<BlogPost>().AsEnumerable().Select(x => new
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
            foreach (var post in allPosts)
            {
                var tagModel = allPosts.SelectMany(t => t.tags).GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count())
                    .Where(pair => allPosts.Single(t => t.id == post.id).tags.Contains(pair.Key)).Select(x => new TagModel()
                    {
                        Name = x.Key,
                        PostCount = x.Value
                    }).ToArray();
                yield return new PostReadModel()
                {
                    Id = post.id,
                    Title = post.title,
                    Text = shorten.Shorten(post.text),
                    Slug = post.slug,
                    Created = post.created.ToDateTimeUtc(),
                    Tags = tagModel,
                    BlogName = post.blog,
                    BlogAuthorDisplayName = post.blogAuthor
                };
            }
        }
    }
}
