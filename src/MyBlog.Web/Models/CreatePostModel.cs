using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyBlog.Core.Entities;

namespace MyBlog.Web.Models
{
    public class ModelBase<T>
    {
        public virtual T GetDomainObject()
        {
            Mapper.CreateMap(GetType(), typeof (T));
            return Mapper.Map<T>(this);
        }
    }

    public class CreatePostModel: ModelBase<BlogPost>
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string TimeZoneId { get; set; }

        public override BlogPost GetDomainObject()
        {
            var result = base.GetDomainObject();
            result.SetCreated(DateTime.UtcNow, TimeZoneId);
            return result;
        }
    }
}