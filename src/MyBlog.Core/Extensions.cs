using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlog.Core
{
    public static class Extensions
    {
        public static double ConvertToUnixTimestamp()
        {
            return Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
    }
}
