using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlog.Core
{
    public static class Tag
    {
        public static string[] Split(string tags)
        {
            return tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        }
    }
}
