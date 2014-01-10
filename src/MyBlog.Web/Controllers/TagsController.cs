using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core.Contracts;
using MyBlog.Core.Models;

namespace MyBlog.Web.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService tagService;
        private readonly IPostService postService;

        public TagsController(ITagService tagService, IPostService postService)
        {
            this.tagService = tagService;
            this.postService = postService;
        }

        [HttpGet]
        public ActionResult Cloud()
        {
            var tags = tagService.Cloud().Select(tm => new { name = tm.Name, url = Url.RouteUrl("PostsByTag", new { name = tm.Name }), postCount = tm.PostCount }).ToArray();
            return Json(new { tags }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PostsFor(string tag)
        {
            return Json("ok");
            //var posts = new List<PostReadModel>();
            //if (string.IsNullOrEmpty(tag))
            //{
            //    posts = postService.List().ToList();
            //}
            //else
            //{
            //    posts = tagService.PostsByTag(tag);
            //}

            //return View(new PostsByTagViewModel()
            //    {
            //        Tag = tag, 
            //        Posts = posts
            //    });
        }
    }
}
