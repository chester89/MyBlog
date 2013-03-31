using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyBlog.Core.Entities;

namespace MyBlog.Web.Models
{
    public class PostListModel
    {
        public ICollection<PostModel> Posts { get; private set; }

        public PostListModel(IEnumerable<BlogPost> posts)
        {
            Posts = Mapper.Map<IEnumerable<BlogPost>, IEnumerable<PostModel>>(posts).ToList();
        }
    }
}