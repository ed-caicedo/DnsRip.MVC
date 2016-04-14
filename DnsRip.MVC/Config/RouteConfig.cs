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
    }
}