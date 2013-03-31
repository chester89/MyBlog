using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core.Contracts;

namespace MyBlog.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ISlugService slugService;

        public ServicesController(ISlugService slugService)
        {
            this.slugService = slugService;
        }

        public ActionResult Coordinates()
        {
            var result = string.Empty;
            string clientIp = string.Empty;
            if (Request.IsLocal)
            {
                clientIp = "195.91.225.65";
            }
            else
            {
                clientIp = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(clientIp))
                {
                    clientIp = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                }
                IPAddress address;
                if (!IPAddress.TryParse(clientIp, out address))
                {
                    throw new ArgumentException("no client address");
                }
            }
            using (var client = new WebClient())
            {
                result = client.DownloadString(string.Format("http://api.ipinfodb.com/v3/ip-city/?key=627092c3e3d7db72c3e47c858a77846196b428d1c222a429592fe00b2dc8d9c4&ip={0}&format=json", clientIp));
            }
            return Content(result, "text/json");
        }

        public ActionResult Timezone(string latitude, string longitude, double timestamp, bool sensor)
        {
            var result = string.Empty;
            var intStamp = Convert.ToInt64(timestamp);
            using (var client = new WebClient())
            {
                result = client.DownloadString(string.Format("https://maps.googleapis.com/maps/api/timezone/json?location={0}&timestamp={1}&sensor={2}",
                    string.Join(",", latitude, longitude), intStamp, sensor.ToString().ToLower()));
            }
            return Content(result, "text/json");
        }
        
        public ActionResult PostSlug(string title)
        {
            var slug = slugService.GenerateSlug(title);
            return Json(new { slug = slug }, JsonRequestBehavior.AllowGet);
        }
    }
}
