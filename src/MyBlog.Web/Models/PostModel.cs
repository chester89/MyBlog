using System;

namespace MyBlog.Web.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        //public string Tags { get; set; }
        public TagModel[] Tags { get; set; }
        public string Slug { get; set; }
        public DateTime Created { get; set; }
        public string BlogName { get; set; }
        public string BlogAuthorDisplayName { get; set; }
    }
}