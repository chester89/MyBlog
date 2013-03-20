using System.Collections.Generic;

namespace MyBlog.Core.Entities
{
    public class Blog
    {
        public virtual int Id { get; private set; }
        public virtual ICollection<BlogPost> Posts { get; set; }
        public virtual User Author { get; set; }

        public Blog()
        {
            Posts = new List<BlogPost>();
        }
    }
}
