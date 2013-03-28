using System;
using AutoMapper;
using MyBlog.Core.Entities;

namespace MyBlog.Web.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
        public DateTime Created { get; set; }

        public static PostModel FromSource(BlogPost post)
        {
            return Mapper.Map<BlogPost, PostModel>(post);
        }
    }
}