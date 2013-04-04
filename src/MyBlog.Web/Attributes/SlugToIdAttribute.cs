using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core.Contracts;

namespace MyBlog.Web.Attributes
{
    public class SlugToIdAttribute : FilterAttribute, IActionFilter
    {
        public ISlugService SlugService { get; set; }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var slug = filterContext.RouteData.Values["slug"] as string;
            if (!string.IsNullOrEmpty(slug))
            {
                filterContext.ActionParameters["id"] = SlugService.GetPostId(slug);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}