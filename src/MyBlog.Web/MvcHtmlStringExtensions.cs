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
            result = result.Replace("value=\"\"", string.Format("data-bind=\"{0}\"", binding));
            return MvcHtmlString.Create(result);
        }
    }
}