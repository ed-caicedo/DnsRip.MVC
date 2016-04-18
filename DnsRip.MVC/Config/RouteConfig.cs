using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace DnsRip.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();
        }

        public static void RegisterConfig(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
    }
}