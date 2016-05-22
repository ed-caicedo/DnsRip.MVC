using System.Collections.Generic;
using DnsRip.Models;

namespace DnsRip.MVC.Responses
{
    public interface IRawRunResponse
    {
        string Error { get; set; }
        bool IsValid { get; set; }
        string Query { get; set; }
        IEnumerable<ResolveResponse> Response { get; set; }
        string Type { get; set; }
    }
}