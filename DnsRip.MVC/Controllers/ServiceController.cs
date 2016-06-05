using System;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using log4net;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DnsRip.MVC.Extensions;

namespace DnsRip.MVC.Controllers
{
    [ValidateAntiForgeryToken]
    public class ServiceController : Controller
    {
        public ServiceController(ILog log, IParseResponseFactory parseResponseFactory, IRunResponseFactory runResponseFactory,
            IRunCsvResponseFactory runCsvResponseFactory, HttpRequestBase httpRequest)
        {
            _log = log;
            _parseResponseFactory = parseResponseFactory;
            _runResponseFactory = runResponseFactory;
            _runCsvResponseFactory = runCsvResponseFactory;
            _httpRequest = httpRequest;

            LogicalThreadContext.Properties["ip"] = _httpRequest.UserHostAddress;
        }

        private readonly ILog _log;
        private readonly IParseResponseFactory _parseResponseFactory;
        private readonly IRunResponseFactory _runResponseFactory;
        private readonly IRunCsvResponseFactory _runCsvResponseFactory;
        private readonly HttpRequestBase _httpRequest;

        [HttpPost]
        [Route("parse")]
        public ActionResult Parse(ParseRequest request)
        {
            _log.Info($"action: Parse; request: {request.Value}");

            if (request.Value == null)
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            var result = _parseResponseFactory.Create(request);

            _log.Info($"action: Parse; result: {result.Parsed}; type: {result.Type}");

            return Json(result);
        }

        [HttpPost]
        [Route("run")]
        public ActionResult Run(RunRequest request)
        {
            foreach (var domain in request.Domains)
                _log.Info($"action: Run; request: {domain}; ip: {_httpRequest.UserHostAddress}");

            var response = _runResponseFactory.Create(request).ToList();

            foreach (var resp in response)
            {
                if (resp.IsValid)
                    _log.Info($"action: Run; result: {resp.Query}; isValid: {resp.IsValid}");
                else
                    _log.Warn($"action: Run; warning: {resp.Error}; isValid: {resp.IsValid}");
            }

            return Json(response);
        }

        [HttpPost]
        [Route("download")]
        public ActionResult Download(RunRequest request)
        {
            foreach (var domain in request.Domains)
                _log.Info($"action: Download; request: {domain}");

            var runCsvResponseStream = _runCsvResponseFactory.Create(request);
            var timestamp = DateTime.UtcNow.ToJsTime();

            _log.Info($"action: Download; streamLength: {runCsvResponseStream.Stream.Length}");

            return File(runCsvResponseStream.Stream, "text/csv", $"dns.rip.{timestamp}.csv");
        }
    }
}