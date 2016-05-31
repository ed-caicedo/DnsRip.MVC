using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileHelpers;

namespace DnsRip.MVC.Responses
{
    public class RunCsvResponseFactory : IDisposable
    {
        public MemoryStream Stream { get; set; }
        private StreamWriter _streamWriter { get; set; }

        public void Create(IEnumerable<RunResponse> runResponse)
        {
            var csv = runResponse.Where(r => r.IsValid).SelectMany(r => r.Records, (response, record) => new RunCsvResponse
            {
                Query = response.Query,
                Type = record.Type,
                Result = record.Result
            });

            var fileHelper = new FileHelperEngine<RunCsvResponse>();

            Stream = new MemoryStream();
            _streamWriter = new StreamWriter(Stream);

            fileHelper.WriteStream(_streamWriter, csv);

            _streamWriter.Flush();
            Stream.Flush();
            Stream.Position = 0;
        }

        public void Dispose()
        {
            Stream.Dispose();
            _streamWriter.Dispose();
        }
    }
}