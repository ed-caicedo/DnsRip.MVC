using System.Collections.Generic;
using DnsRip.Models;
using DnsRip.MVC.Interfaces;

namespace DnsRip.MVC.Responses
{
    public class ParseResponse : IParseResponse
    {
        public ParseResponse(ParseResult parsed, IAdditionalHosts addHosts)
        {
            Input = parsed.Input;
            Evaluated = parsed.Evaluated;
            Parsed = parsed.Parsed;
            Type = parsed.Type.ToString();
            Additional = parsed.Type == InputType.Hostname ? addHosts.Find(parsed.Parsed) : null;
        }

        public string Input { get; set; }
        public string Evaluated { get; set; }
        public string Parsed { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Additional { get; set; }
    }
}