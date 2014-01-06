using System;
using NodaTime;

namespace MyBlog.Core.Entities
{
    public interface IStarted
    {
        ZonedDateTime Created { get; }
        void SetCreated(DateTime utcDate, string timeZoneId);
    }

    public class BlogPost: IStarted
    {
        private Zone timeZone { get; set; }
        public virtual int Id { get; protected set; }

        /// <summary>
        /// was posted from this timezone at this moment in time
        /// </summary>
        public virtual ZonedDateTime Created
        {
            get
            {
                return new ZonedDateTime(new Instant(timeZone.InstantTicks), DateTimeZoneProviders.Tzdb[timeZone.ZoneId]);
            } 
        }

        public virtual DateTime CreatedInZone(string timeZoneId)
        {
            var targetZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            return Created.WithZone(targetZone).ToDateTimeUnspecified();
        }

        public virtual string Title { get; set; }
        public virtual string Text { get; set; }
        public virtual string Tags { get; set; }
        public virtual string Slug { get; set; }
        public virtual Blog Blog { get; set; }

        public BlogPost()
        {
            timeZone = new Zone();
        }

        /// <param name="dateTime">datetime should be provided in UTC</param>
        /// <param name="timeZoneId"></param>
        public virtual void SetCreated(DateTime dateTime, string timeZoneId)
        {
            timeZone = new Zone
            {
                InstantTicks = Instant.FromDateTimeUtc(dateTime).Ticks,
                ZoneId = timeZoneId
            };
        }
    }
}
