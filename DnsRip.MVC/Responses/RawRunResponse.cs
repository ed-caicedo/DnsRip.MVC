using System.Collections.Generic;
using DnsRip.Models;

namespace DnsRip.MVC.Responses
{
    public class RawRunResponse : IRawRunResponse
    {
        public string Query { get; set; }
        public string Type { get; set; }
        public bool IsValid { get; set; }
        public string Error { get; set; }
        public IEnumerable<ResolveResponse> Response { get; set; }
    }
}