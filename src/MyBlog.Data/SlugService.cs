using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;

namespace MyBlog.Data
{
    public class SlugService: ISlugService
    {
        private readonly IRepository<BlogPost> repository;

        public SlugService(IRepository<BlogPost> repository)
        {
            this.repository = repository;
        }

        public int GetPostId(string slug)
        {
            var post = repository.Get(x => x.Slug == slug).SingleOrDefault();
            if (post != null)
            {
                return post.Id;
            }
            throw new ArgumentException(string.Format("This slug wasn't found - {0}", slug));
        }

        public string GenerateSlug(string title)
        {
            var firstAttempt = title.Replace(" ", "-").Trim();

            if (repository.GetAll().Any(bp => bp.Slug == firstAttempt))
            {
                firstAttempt += "";
            }
            return firstAttempt;
        }
    }
}
