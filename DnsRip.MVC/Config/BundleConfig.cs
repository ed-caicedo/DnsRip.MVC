using System.Web.Optimization;

namespace DnsRip.MVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new StyleBundle("~/assets/css/default.bundle/")
                .Include("~/assets/css/lib/bootstrap.css")
                .Include("~/assets/css/lib/bootstrap-theme.css"));

            bundles.Add(new ScriptBundle("~/assets/js/default.bundle/")
                .Include("~/assets/js/lib/jquery.js")
                .Include("~/assets/js/lib/bootstrap.js"));
        }
    }
}