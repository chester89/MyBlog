using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBlog.Web.Models
{
    public class PostListModel
    {
        public ICollection<PostModel> Posts { get; set; }

        public PostListModel()
        {
            Posts = new List<PostModel>();
        }
    }
}