using System;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using log4net;
using System.Linq;
using System.Web;
using System.Web.Http;
using Mvc = System.Web.Mvc;

namespace DnsRip.MVC.Controllers
{
    public class ServiceController : ApiController
    {
        public ServiceController(ILog log, IParseResponseFactory parseResponseFactory, IRunResponseFactory runResponseFactory,
            HttpRequestBase httpRequest)
        {
            _log = log;
            _parseResponseFactory = parseResponseFactory;
            _runResponseFactory = runResponseFactory;
            _httpRequest = httpRequest;
        }

        private readonly ILog _log;
        private readonly IParseResponseFactory _parseResponseFactory;
        private readonly IRunResponseFactory _runResponseFactory;
        private readonly HttpRequestBase _httpRequest;

        [HttpPost]
        [Route("parse")]
        [Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult Parse(ParseRequest request)
        {
            _log.Debug($"action: Parse; request: {request.Value}; ip: {_httpRequest.UserHostAddress}");

            if (request.Value == null)
                return Ok();

            var result = _parseResponseFactory.Create(request);

            _log.Debug($"action: Parse; result: {result.Parsed}; type: {result.Type}");

            return Ok(result);
        }

        [HttpPost]
        [Route("run")]
        [Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult Run(RunRequest request)
        {
            foreach (var domain in request.Domains)
                _log.Debug($"action: Run; request: {domain}; ip: {_httpRequest.UserHostAddress}");

            var response = _runResponseFactory.Create(request).ToList();

            foreach (var resp in response)
                _log.Debug($"action: Run; result: {resp.Query}; isValid: {resp.IsValid}; ip: {_httpRequest.UserHostAddress}");

            return Ok(response);
        }
    }
}