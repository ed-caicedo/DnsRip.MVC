using System.Web.Optimization;

namespace DnsRip.MVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            bundles.Add(new StyleBundle("~/assets/css/default.bundle/")
                .Include("~/assets/css/lib/fonts.css")
                .Include("~/assets/css/lib/bootstrap.css")
                .Include("~/assets/css/main.css"));

            bundles.Add(new ScriptBundle("~/assets/js/default.bundle/")
                .Include("~/assets/js/lib/jquery.js")
                .Include("~/assets/js/lib/bootstrap.js")
                .Include("~/assets/js/lib/knockout.js")
                .Include("~/assets/js/lib/typewatch.js")
                .Include("~/assets/js/utl/utilities.js")
                .Include("~/assets/js/main.js"));
        }
    }
}