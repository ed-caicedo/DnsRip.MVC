using System.Collections.Generic;
using DnsRip.MVC.Requests;
using DnsRip.MVC.Responses;

namespace DnsRip.MVC.Interfaces
{
    public interface IRawRunResponseFactory
    {
        IEnumerable<RawRunResponse> Create(RunRequest request);
    }
}