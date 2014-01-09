using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Models;

namespace MyBlog.Core.Contracts
{
    public interface ITagService
    {
        void CalculateTagCounters();
        TagModel[] ByPost(int postId);
        TagModel[] Cloud();
    }
}
