using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;

namespace MyBlog.Core.Contracts
{
    public interface IPostService
    {
        void AddNew(BlogPost newPost, int blogId);
        PostReadModel Read(int postId);
        IEnumerable<PostReadModel> List();
        IEnumerable<PostReadModel> ByTag(string tag);
    }
}
