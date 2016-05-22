using System.Collections.Generic;

namespace DnsRip.MVC.Interfaces
{
    public interface IParseResponse
    {
        IEnumerable<string> Additional { get; set; }
        string Evaluated { get; set; }
        string Input { get; set; }
        string Parsed { get; set; }
        string Type { get; set; }
    }
}