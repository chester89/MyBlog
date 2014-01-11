using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core.Contracts;
using MyBlog.Web.Models;

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
            var posts = (string.IsNullOrEmpty(tag) ? postService.List(): postService.ByTag(tag)).ToList();

            return View(new PostsByTagViewModel()
                {
                    Tag = tag,
                    Posts = AutoMapper.Mapper.Map<ICollection<PostModel>>(posts) 
                });
        }
    }
}
