using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions2;
using MyBlog.Core.Contracts;

namespace MyBlog.Data
{
    public class ShortenAlgorithm: IShortenAlgorithm
    {
        public string Shorten(string content)
        {
            var delimiterPositions = content.SubstringPositions(Environment.NewLine).ToArray();
            if (delimiterPositions.Count() >= 2)
            {
                var substring = content.SubstringOnIndex(0, delimiterPositions[1] - 1);
                return substring;
            }
            return content;
        }
    }
}
