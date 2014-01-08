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
            //CKEditor adds NewLine at the start of the string, so - find a 2nd occurrence of NewLine in a string and truncate string after it
            var delimiterPosition = content.IndexOf(Environment.NewLine, StringComparison.InvariantCulture);
            return content.Substring(0, delimiterPosition == -1 ? content.Length : delimiterPosition);
        }
    }
}
