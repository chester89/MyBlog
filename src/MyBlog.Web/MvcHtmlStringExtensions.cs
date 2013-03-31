using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Web
{
    public static class MvcHtmlStringExtensions
    {
        public static MvcHtmlString DataBind(this MvcHtmlString source, string binding)
        {
            var result = source.ToHtmlString();
            result = result.Substring(0, result.Length - result.LastIndexOf(' ') + 1) + "data-bind=" + binding + result.Substring(result.LastIndexOf(' ') + 1);
            return MvcHtmlString.Create(result);
        }
    }
}