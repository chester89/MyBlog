using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using NHibernate;

namespace MyBlog.Data.Repositories
{
    public class PostRepository : NhRepositoryBase<BlogPost>, IPostRepository
    {
        public PostRepository(ISession session) : base(session)
        {
        }

        public void AddNew(BlogPost newPost, int blogId)
        {
            var blog = Session.Get<Blog>(blogId);
            newPost.Blog = blog;
            Session.SaveOrUpdate(newPost);
        }
    }
}
