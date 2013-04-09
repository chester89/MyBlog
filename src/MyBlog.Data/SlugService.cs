using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace MyBlog.Data
{
    public class SlugService: ISlugService
    {
        private readonly ISession session;

        public SlugService(ISession session)
        {
            this.session = session;
        }

        public int GetPostId(string slug)
        {
            var post = session.Query<BlogPost>().SingleOrDefault(x => x.Slug == slug);
            if (post != null)
            {
                return post.Id;
            }
            throw new ArgumentException(string.Format("This slug wasn't found - {0}", slug));
        }

        public string GenerateSlug(string title)
        {
            var firstAttempt = title.Replace(" ", "-").Trim();

            if (session.Query<BlogPost>().Any(bp => bp.Slug == firstAttempt))
            {
                firstAttempt += "";
            }
            return firstAttempt;
        }
    }
}
