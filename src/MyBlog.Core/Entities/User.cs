namespace MyBlog.Core.Entities
{
    public class User
    {
        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
    }
}
