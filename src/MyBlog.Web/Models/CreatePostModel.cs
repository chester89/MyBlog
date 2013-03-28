﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.Models
{
    public class CreatePostModel
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string TimeZoneId { get; set; }
    }
}