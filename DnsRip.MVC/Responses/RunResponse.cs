using System.Collections.Generic;

namespace DnsRip.MVC.Responses
{
    public class RunResponse
    {
        public string Query { get; set; }
        public IEnumerable<RunRecord> Records { get; set; }
        public bool IsValid { get; set; }
        public string Error { get; set; }
    }
}