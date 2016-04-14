using System.Web.Mvc;

namespace DnsRip.MVC
{
    public class DefaultController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}