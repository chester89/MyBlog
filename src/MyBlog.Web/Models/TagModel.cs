using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.Models
{
    public class TagModel
    {
        public String Name { get; set; }
        public Int32 PostCount { get; set; }
    }
}