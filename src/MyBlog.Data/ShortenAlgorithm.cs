using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;

namespace MyBlog.Data
{
    public class ShortenAlgorithm: IShortenAlgorithm
    {
        public string Shorten(string content)
        {
            var delimiterPosition = content.IndexOf(Environment.NewLine, StringComparison.InvariantCulture);
            return content.Substring(delimiterPosition == -1 ? 0 : delimiterPosition);
        }
    }
}
