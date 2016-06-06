using DnsRip.MVC.Requests;
using DnsRip.MVC.Responses;
using System.Collections.Generic;

namespace DnsRip.MVC.Interfaces
{
    public interface IRawRunResponseFactory
    {
        IEnumerable<RawRunResponse> Create(RunRequest request);
    }
}