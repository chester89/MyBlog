using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Entities;

namespace MyBlog.Core.Contracts
{
    public interface IPostRepository
    {
        void AddNew(BlogPost newPost, int blogId);
    }
}
