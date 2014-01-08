using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public TagModel[] Tags { get; set; }
        public string Slug { get; set; }
        public DateTime Created { get; set; }
        public string BlogName { get; set; }
        public string BlogAuthorDisplayName { get; set; }
        public string DisqusShortName { get; set; }
    }

    public class TagModel
    {
        public String Name { get; set; }
        public Int32 PostCount { get; set; }
    }
}