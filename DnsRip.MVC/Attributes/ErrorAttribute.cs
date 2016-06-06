using log4net;
using System.Web.Mvc;

namespace DnsRip.MVC.Attributes
{
    public class ErrorAttribute : HandleErrorAttribute
    {
        public ILog Log { get; set; }

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Log.Error($"ip: { filterContext.RequestContext.HttpContext.Request.UserHostAddress}", filterContext.Exception);
        }
    }
}