using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;
using NHibernate;

namespace MyBlog.Data.Repositories
{
    public class PostService : NhRepositoryBase<BlogPost>, IPostService
    {
        private readonly IShortenAlgorithm shorten;
        private readonly ITagService tagService;

        public PostService(ISession session, IShortenAlgorithm shorten, ITagService tagService) : base(session)
        {
            this.shorten = shorten;
            this.tagService = tagService;
        }

        public void AddNew(BlogPost newPost, int blogId)
        {
            var blog = Session.Get<Blog>(blogId);
            newPost.Blog = blog;
            Update(newPost);
            tagService.UpdateTags(newPost);
        }

        public PostReadModel Read(int postId)
        {
            var post = Mapper.Map<PostReadModel>(Get(postId));
            post.Tags = tagService.ByPost(postId);
            return post;
        }

        public IEnumerable<PostReadModel> List()
        {
            var posts = GetAll().OrderByDescending(x => x.Created.ToDateTimeUtc()).Take(20).Select(Mapper.Map<PostReadModel>).ToList();
            posts.ForEach(p =>
                {
                    p.Tags = tagService.ByPost(p.Id);
                    p.Text = shorten.Shorten(p.Text);
                });
            return posts;
        }
    }
}
