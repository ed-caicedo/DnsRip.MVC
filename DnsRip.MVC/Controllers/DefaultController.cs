using System.Web.Mvc;

namespace DnsRip.MVC.Controllers
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