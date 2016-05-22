using System.Collections.Generic;

namespace DnsRip.MVC.Responses
{
    public interface IRunResponse
    {
        string Error { get; set; }
        bool IsValid { get; set; }
        string Query { get; set; }
        IEnumerable<RunRecord> Records { get; set; }
    }
}