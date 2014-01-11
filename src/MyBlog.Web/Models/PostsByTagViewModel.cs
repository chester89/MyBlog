using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.Models
{
    public class PostsByTagViewModel: PostListModel
    {
        public String Tag { get; set; }
    }
}