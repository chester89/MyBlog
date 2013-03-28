using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Web.Models;

namespace MyBlog.Web.Controllers
{
    public class PostsController: Controller
    {
        private readonly IRepository<BlogPost> postRepository;

        public PostsController(IRepository<BlogPost> postRepository)
        {
            this.postRepository = postRepository;
        }

        [HttpGet]
        public ActionResult Default(int id)
        {
            var post = postRepository.Get(id);
            return View(PostModel.FromSource(post));
        }

        [HttpGet]
        public ActionResult List()
        {
            var posts = postRepository.GetAll().Take(20).AsEnumerable().OrderByDescending(p => p.Created.ToDateTimeUtc());
            return View(new PostListModel(posts));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreatePostModel());
        }

        [HttpPost]
        public ActionResult Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            postRepository.Add(model.GetDomainObject());

            return RedirectToAction("List");
        }
    }
}