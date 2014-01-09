using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using AutoMapper;
using MyBlog.Core;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Web.Attributes;
using MyBlog.Web.Models;

namespace MyBlog.Web.Controllers
{
    public class PostsController: Controller
    {
        private readonly IPostService postService;
        private readonly IRepository<BlogPost> postReader;
        private readonly IRepository<Blog> blogRepository;

        public PostsController(IPostService postService, IRepository<BlogPost> postReader, IRepository<Blog> blogRepository)
        {
            this.postService = postService;
            this.postReader = postReader;
            this.blogRepository = blogRepository;
        }

        [HttpGet]
        [SlugToId]
        public ActionResult Default(int id)
        {
            return View(Mapper.Map<PostModel>(postService.Read(id)));
        }

        [HttpGet]
        //filter by blogId here
        public ActionResult List()
        {
            var posts = postService.List();
            return View(Mapper.Map<PostListModel>(posts));
        }

        [HttpGet]
        //authentication here
        public ActionResult Create()
        {
            return View(new CreatePostModel() { /*specify calculated BlogId here based on current user*/ BlogId = 1 });
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

            postService.AddNew(model.GetDomainObject(), model.BlogId);

            return RedirectToAction("List");
        }

        public ActionResult RssPosts()
        {
            var urlBase = Url.GetUrlBase();
            var blog = blogRepository.GetAll().SingleOrDefault();
            var posts = postReader.Get(p => p.Blog == blog).Take(20).AsEnumerable().OrderByDescending(x => x.Created.ToDateTimeUtc()).ToList();
            var postItems = posts.Select(p => new SyndicationItem(p.Title, p.Text, new Uri(string.Format("{0}{1}{2}", urlBase, Messages.Posts_View_SlugPrefix, p.Slug))));

            var feed = new SyndicationFeed(blog.Name, "Default blog description", new Uri(urlBase) , postItems) 
            {
                Language = "en-US"
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
        }
    }
}