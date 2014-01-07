using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Web.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Default()
        {
            return RedirectToAction("List", "Posts");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}