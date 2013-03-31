using System;
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

        /// <summary>
        /// was posted from this timezone at this moment in time
        /// </summary>
        public virtual ZonedDateTime Created
        {
            get { return new ZonedDateTime(new Instant(timeZone.InstantTicks), DateTimeZoneProviders.Default.GetZoneOrNull(timeZone.ZoneId)); } 
        }
        public virtual string Title { get; set; }
        public virtual string Text { get; set; }
        public virtual string Tags { get; set; }
        public virtual string Slug { get; set; }

        public virtual void SetCreated(DateTime dateTime, string timeZone)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("datetime provided should be in UTC", "dateTime");
            }
            if (string.IsNullOrEmpty(timeZone))
            {
                throw new ArgumentException("timezone should be set", "timeZone");
            }
            this.timeZone = new Zone
            {
                InstantTicks = Instant.FromDateTimeUtc(dateTime.ToUniversalTime()).Ticks,
                ZoneId = timeZone
            };
        }
    }
}
