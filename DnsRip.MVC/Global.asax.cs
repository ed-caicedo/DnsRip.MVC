using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using log4net.Config;

namespace DnsRip.MVC
{

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(RouteConfig.RegisterConfig);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.Configure();
        }
    }
}