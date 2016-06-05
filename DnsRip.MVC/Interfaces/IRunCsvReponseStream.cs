using System;
using System.Collections.Generic;
using System.IO;
using DnsRip.MVC.Responses;

namespace DnsRip.MVC.Interfaces
{
    public interface IRunCsvReponseStream : IDisposable
    {
        MemoryStream Stream { get; set; }
        
        void Initialize(IEnumerable<RunResponse> runResponse);
    }
}