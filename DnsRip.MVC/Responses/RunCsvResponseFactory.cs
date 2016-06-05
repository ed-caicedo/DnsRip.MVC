using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using System;

namespace DnsRip.MVC.Responses
{
    public class RunCsvResponseFactory : IRunCsvResponseFactory
    {
        public RunCsvResponseFactory(IRunResponseFactory runResponseFactory, IRunCsvReponseStream runCsvReponseStream)
        {
            _runResponseFactory = runResponseFactory;
            _runCsvReponseStream = runCsvReponseStream;
        }

        private readonly IRunResponseFactory _runResponseFactory;
        private readonly IRunCsvReponseStream _runCsvReponseStream;

        public IRunCsvReponseStream Create(RunRequest request)
        {
            var runResponse = _runResponseFactory.Create(request);
            _runCsvReponseStream.Initialize(runResponse);
            return _runCsvReponseStream;
        }
    }
}