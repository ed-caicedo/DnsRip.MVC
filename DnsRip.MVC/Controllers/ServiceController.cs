using System.Configuration;
using System.Diagnostics;
using DnsRip.MVC.Models;
using DnsRip.MVC.Utilities;
using System.Web.Http;

namespace DnsRip.MVC.Controllers
{
    public class ServiceController : ApiController
    {
        private readonly string[] _subdomains = new[]
        {
            "www",
            "m",
            "blog",
            "ftp",
            "imap",
            "pop",
            "smtp",
            "mail",
            "webmail"
        };

        [HttpPost]
        [Route("parse")]
        public IHttpActionResult Parse(ParseRequest request)
        {
            var log = log4net.LogManager.GetLogger(GetType());

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