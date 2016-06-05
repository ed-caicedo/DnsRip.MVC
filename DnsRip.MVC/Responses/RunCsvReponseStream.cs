using DnsRip.MVC.Interfaces;
using FileHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DnsRip.MVC.Responses
{
    public class RunCsvReponseStream<T> : IRunCsvReponseStream where T : RunCsvResponse, new()
    {
        public RunCsvReponseStream(IFileHelperEngine<T> fileHelperEngine)
        {
            _fileHelperEngine = fileHelperEngine;
        }

        private readonly IFileHelperEngine<T> _fileHelperEngine;

        public MemoryStream Stream { get; set; }
        private StreamWriter _streamWriter { get; set; }

        public void Initialize(IEnumerable<RunResponse> runResponse)
        {
            var csv = runResponse.Where(r => r.IsValid).SelectMany(r => r.Records, (response, record) => new T
            {
                Query = response.Query,
                Type = record.Type,
                Result = record.Result
            });
            
            Stream = new MemoryStream();

            _streamWriter = new StreamWriter(Stream);
            _fileHelperEngine.WriteStream(_streamWriter, csv);
            _streamWriter.Flush();

            Stream.Flush();
            Stream.Position = 0;
        }

        public void Dispose()
        {
            _streamWriter.Dispose();
            Stream.Dispose();
        }
    }
}