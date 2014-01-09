using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;

namespace MyBlog.Core.Contracts
{
    public interface ITagService
    {
        void UpdateTags(BlogPost newPost);
        TagModel[] ByPost(int postId);
        TagModel[] Cloud();
    }
}
