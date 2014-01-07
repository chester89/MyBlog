using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlog.Core.Models
{
    public class PostReadModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public TagModel[] Tags { get; set; }
        public string Slug { get; set; }
        public DateTime Created { get; set; }
        public string BlogName { get; set; }
        public string BlogAuthorDisplayName { get; set; }
    }
}
