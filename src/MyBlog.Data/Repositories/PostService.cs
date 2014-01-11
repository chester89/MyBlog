using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;

namespace MyBlog.Data.Repositories
{
    public class PostService: IPostService
    {
        private readonly IRepository<Blog> blogRepository;
        private readonly IRepository<BlogPost> postRepository;
        private readonly IShortenAlgorithm shorten;
        private readonly ITagService tagService;
        private readonly ICacheRepository cache;

        public PostService(IRepository<Blog> blogRepository, IRepository<BlogPost> postRepository, IShortenAlgorithm shorten, ITagService tagService, ICacheRepository cache)
        {
            this.blogRepository = blogRepository;
            this.postRepository = postRepository;
            this.shorten = shorten;
            this.tagService = tagService;
            this.cache = cache;
        }

        public void AddNew(BlogPost newPost, int blogId)
        {
            var blog = blogRepository.Get(blogId);
            newPost.Blog = blog;
            postRepository.Update(newPost);
            tagService.UpdateTags(newPost);
            cache.AddNewPost(newPost);
        }

        public PostReadModel Read(int postId)
        {
            var post = Mapper.Map<PostReadModel>(postRepository.Get(postId));
            post.Tags = tagService.ByPost(postId);
            return post;
        }

        public IEnumerable<PostReadModel> List()
        {
            var posts = postRepository.GetAll().AsEnumerable().OrderByDescending(x => x.Created.ToDateTimeUtc()).Take(20).Select(Mapper.Map<PostReadModel>).ToList();
            posts.ForEach(p =>
                {
                    p.Tags = tagService.ByPost(p.Id);
                    p.Text = shorten.Shorten(p.Text);
                });
            return posts;
        }

        public IEnumerable<PostReadModel> ByTag(string tag)
        {
            var postIds = cache.TagsByPost.Where(x => x.Value.Contains(tag)).Select(x => x.Key);
            var posts = cache.Posts.Where(p => postIds.Contains(p.Id)).Select(Mapper.Map<PostReadModel>).ToList();
            posts.ForEach(p =>
            {
                p.Tags = tagService.ByPost(p.Id);
                p.Text = shorten.Shorten(p.Text);
            });
            return posts;
        }
    }
}
