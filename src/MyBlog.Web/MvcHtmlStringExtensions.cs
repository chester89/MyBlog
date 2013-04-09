using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Web
{
    public static class HtmlExtensions
    {
        public static string GetUrlBase(this UrlHelper url)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            return string.Format("{0}://{1}", requestUrl.Scheme, requestUrl.Authority);
        }
    }
}