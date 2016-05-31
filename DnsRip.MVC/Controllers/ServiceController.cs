﻿using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using log4net;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DnsRip.MVC.Controllers
{
    [ValidateAntiForgeryToken]
    public class ServiceController : Controller
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

        public ILog Log { get; set; }

        [HttpPost]
        [Route("parse")]
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
            return null;
        }
    }
}