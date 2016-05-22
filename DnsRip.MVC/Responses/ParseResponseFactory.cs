using DnsRip.Interfaces;
using DnsRip.Models;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;

namespace DnsRip.MVC.Responses
{
    public class ParseResponseFactory : IParseResponseFactory
    {
        public ParseResponseFactory(IParser parser, IAdditionalHosts addHosts)
        {
            _parser = parser;
            _addHosts = addHosts;
        }

        private readonly IParser _parser;
        private readonly IAdditionalHosts _addHosts;

        public ParseResponse Create(ParseRequest request)
        {
            var parsed = _parser.Parse(request.Value);
            return new ParseResponse(parsed, _addHosts);
        }
    }
}