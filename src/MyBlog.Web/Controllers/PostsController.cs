using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using MyBlog.Core;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Web.Attributes;
using MyBlog.Web.Models;

namespace MyBlog.Web.Controllers
{
    public class PostsController: Controller
    {
        private readonly IPostRepository postRepository;
        private readonly IRepository<BlogPost> postReader;
        private readonly IRepository<Blog> blogRepository;
        private readonly IShortenAlgorithm shortenAlgorithm;

        public PostsController(IPostRepository postRepository, IRepository<BlogPost> postReader, IRepository<Blog> blogRepository, IShortenAlgorithm shortenAlgorithm)
        {
            this.postRepository = postRepository;
            this.postReader = postReader;
            this.blogRepository = blogRepository;
            this.shortenAlgorithm = shortenAlgorithm;
        }

        [HttpGet]
        [SlugToId]
        public ActionResult Default(int id)
        {
            var post = postReader.Get(id);
            return View(PostModel.FromSource(post));
        }

        [HttpGet]
        //filter by blogId here
        public ActionResult List()
        {
            var posts = postReader.GetAll().Take(20).AsEnumerable().OrderByDescending(p => p.Created.ToDateTimeUtc()).ToList();
            posts.ForEach(p => p.Text = shortenAlgorithm.Shorten(p.Text));
            return View(new PostListModel(posts));
        }

        [HttpGet]
        //authentication here
        public ActionResult Create()
        {
            return View(new CreatePostModel() { BlogId = 1 });
        }

        [HttpPost]
        [ValidateInput(false)]
        //authentication here
        public ActionResult Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            postRepository.AddNew(model.GetDomainObject(), model.BlogId);

            return RedirectToAction("List");
        }

        public ActionResult RssPosts()
        {
            var urlBase = Url.GetUrlBase();
            var blog = blogRepository.GetAll().SingleOrDefault();
            var posts = postReader.Get(p => p.Blog == blog).Take(20).AsEnumerable().OrderByDescending(x => x.Created.ToDateTimeUtc()).ToList();
            var postItems = posts.Select(p => new SyndicationItem(p.Title, p.Text, new Uri(string.Format("{0}{1}{2}", urlBase, Messages.Posts_View_SlugPrefix, p.Slug))));

            var feed = new SyndicationFeed(blog.Name, "No description", new Uri(urlBase) , postItems) 
            {
                Language = "en-US"
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
        }
    }
}