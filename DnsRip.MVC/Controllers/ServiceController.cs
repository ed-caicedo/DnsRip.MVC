using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using log4net;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DnsRip.MVC.Controllers
{
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
        }

        private readonly ILog _log;
        private readonly IParseResponseFactory _parseResponseFactory;
        private readonly IRunResponseFactory _runResponseFactory;
        private readonly IRunCsvResponseFactory _runCsvResponseFactory;
        private readonly HttpRequestBase _httpRequest;

        public ILog Log { get; set; }

        [HttpPost]
        [Route("parse")]
        [ValidateAntiForgeryToken]
        public ActionResult Parse(ParseRequest request)
        {
            _log.Info($"action: Parse; request: {request.Value}; ip: {_httpRequest.UserHostAddress}");

            if (request.Value == null)
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            var result = _parseResponseFactory.Create(request);

            _log.Info($"action: Parse; result: {result.Parsed}; type: {result.Type}");

            return Json(result);
        }

        [HttpPost]
        [Route("run")]
        [ValidateAntiForgeryToken]
        public ActionResult Run(RunRequest request)
        {
            foreach (var domain in request.Domains)
                _log.Info($"action: Run; request: {domain}; ip: {_httpRequest.UserHostAddress}");

            var response = _runResponseFactory.Create(request).ToList();

            foreach (var resp in response)
            {
                if (resp.IsValid)
                    _log.Info($"action: Run; result: {resp.Query}; isValid: {resp.IsValid}; ip: {_httpRequest.UserHostAddress}");
                else
                    _log.Warn($"action: Run; warning: {resp.Error}; isValid: {resp.IsValid}; ip: {_httpRequest.UserHostAddress}");
            }

            return Json(response);
        }

        [HttpPost]
        [Route("download")]
        public ActionResult Download(RunRequest request)
        {
            var runCsvResponseStream = _runCsvResponseFactory.Create(request);
            return File(runCsvResponseStream.Stream, "text/csv", "dns.rip.csv");
        }
    }
}