using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core.Contracts;

namespace MyBlog.Web.Attributes
{
    public class SlugToIdAttribute : ActionFilterAttribute
    {
        public ISlugService SlugService { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var slug = filterContext.RouteData.Values["slug"] as string;
            if (slug != null)
            {
                filterContext.ActionParameters["id"] = SlugService.GetPostId(slug);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}