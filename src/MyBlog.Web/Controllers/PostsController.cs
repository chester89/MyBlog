using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core;
using MyBlog.Web.Models;

namespace MyBlog.Web.Controllers
{
    public class PostsController: Controller
    {
        [HttpGet]
        public ActionResult List(string blogName)
        {
            return Content("OK");
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
            return Content("OK");
        }
    }
}