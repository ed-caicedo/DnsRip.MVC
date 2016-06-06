using DnsRip.MVC.Requests;
using DnsRip.MVC.Responses;
using System.Collections.Generic;

namespace DnsRip.MVC.Interfaces
{
    public interface IRunResponseFactory
    {
        IEnumerable<RunResponse> Create(RunRequest request);
    }
}