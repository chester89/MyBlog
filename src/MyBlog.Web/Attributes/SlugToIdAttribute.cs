using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core.Contracts;
using StructureMap;

namespace MyBlog.Web.Attributes
{
    [DebuggerDisplay("SlugToId")]
    public class SlugToIdAttribute : FilterAttribute, IActionFilter
    {
        public ISlugService SlugService { get; set; }

        public SlugToIdAttribute()
        {
            Console.WriteLine();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var slug = filterContext.RouteData.Values["slug"] as string;
            if (slug != null)
            {
                filterContext.ActionParameters["id"] = SlugService.GetPostId(slug);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}