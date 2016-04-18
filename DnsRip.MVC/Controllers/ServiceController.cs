using DnsRip.MVC.Models;
using System.Web.Http;

namespace DnsRip.MVC.Controllers
{
    public class ServiceController : ApiController
    {
        [HttpPost]
        [Route("parse")]
        public IHttpActionResult Parse(ParseRequest request)
        {
            var dnsRip = new DnsRip.Parser();
            return Ok(dnsRip.Parse(request.Text));
        }
    }
}