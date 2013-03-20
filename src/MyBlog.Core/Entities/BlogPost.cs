using NodaTime;

namespace MyBlog.Core.Entities
{
    public interface IStarted
    {
        ZonedDateTime Created { get; }
    }

    public class BlogPost: IStarted
    {
        private Zone timeZone;
        public virtual int Id { get; private set; }

        public virtual ZonedDateTime Created
        {
            get { return new ZonedDateTime(new Instant(timeZone.InstantTicks), DateTimeZoneProviders.Default.GetZoneOrNull(timeZone.ZoneId)); } 
        }
        public virtual string Title { get; set; }
        public virtual string Text { get; set; }
        public virtual string Tags { get; set; }
    }
}
