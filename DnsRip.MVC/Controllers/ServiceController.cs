using DnsRip.MVC.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DnsRip.MVC.Models;

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