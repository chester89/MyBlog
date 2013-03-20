using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core;

namespace MyBlog.Web.Controllers
{
    public class ServicesController : Controller
    {
        public ActionResult GetCoordinates()
        {
            var result = string.Empty;
            var apiKey = "627092c3e3d7db72c3e47c858a77846196b428d1c222a429592fe00b2dc8d9c4";
            using(var client = new WebClient())
            {
                result = client.DownloadString(string.Format("http://api.ipinfodb.com/v3/ip-city/?key={0}&format=json", apiKey));
            }
            return Content(result, "text/json");
        }

        public ActionResult GetTimeZone(string latitude, string longitude)
        {
            var result = string.Empty;
            var coordinates = string.Join(",", latitude, longitude);
            var timestamp = Extensions.ConvertToUnixTimestamp();
            using (var client = new WebClient())
            {
                result = client.DownloadString(string.Format("https://maps.googleapis.com/maps/api/timezone/json?location={0}&timestamp={1}&sensor=false", coordinates, timestamp));
            }
            return Content(result, "text/json");
        }
    }
}
