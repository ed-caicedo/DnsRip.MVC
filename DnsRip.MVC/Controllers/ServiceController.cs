using DnsRip.MVC.Models;
using DnsRip.MVC.Utilities;
using log4net;
using System.Web;
using System.Web.Http;

namespace DnsRip.MVC.Controllers
{
    public class ServiceController : ApiController
    {
        private readonly ILog _log;

        public ServiceController(ILog log)
        {
            _log = log;
        }

        [HttpPost]
        [Route("parse")]
        public IHttpActionResult Parse(ParseRequest request)
        {
            _log.Debug($"parse1: {request.Value}; ip: {HttpContext.Current.Request.UserHostAddress}");

            if (request.Value == null)
                return Ok();

            var dnsRip = new DnsRip.Parser();
            var parsed = dnsRip.Parse(request.Value);

            var result = new
            {
                parsed.Input,
                parsed.Evaluated,
                parsed.Parsed,
                Type = parsed.Type.ToString(),
                Additional = parsed.Type == InputType.Hostname ? AdditionalHosts.Find(parsed.Parsed) : null
            };

            return Ok(result);
        }
    }
}