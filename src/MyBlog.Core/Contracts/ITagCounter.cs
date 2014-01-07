using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlog.Core.Contracts
{
    public interface ITagCounter
    {
        IDictionary<String, Int32> ByPost(int postId);
    }
}
