using DnsRip.MVC.Models;
using log4net;
using System.Web;
using System.Web.Http;
using DnsRip.Interfaces;
using DnsRip.MVC.Interfaces;

namespace DnsRip.MVC.Controllers
{
    public class ServiceController : ApiController
    {
        public ServiceController(ILog log, IParser parser, IParseResposeFactory response, HttpRequestBase httpRequest)
        {
            _log = log;
            _parser = parser;
            _response = response;
            _httpRequest = httpRequest;
        }

        private readonly ILog _log;
        private readonly IParser _parser;
        private readonly IParseResposeFactory _response;
        private readonly HttpRequestBase _httpRequest;

        [HttpPost]
        [Route("parse")]
        public IHttpActionResult Parse(ParseRequest request)
        {
            _log.Debug($"parse: {request.Value}; ip: {_httpRequest.UserHostAddress}");

            if (request.Value == null)
                return Ok();
            
            var parsed = _parser.Parse(request.Value);
            var result = _response.Create(parsed);

            return Ok(result);
        }
    }
}