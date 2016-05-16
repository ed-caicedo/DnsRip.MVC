using DnsRip.Models;
using DnsRip.MVC.Interfaces;

namespace DnsRip.MVC.Models
{
    public class ParseResponseFactory : IParseResposeFactory
    {
        public ParseResponseFactory(IAdditionalHosts addHosts)
        {
            _addHosts = addHosts;
        }

        public IParseResponse Create(ParseResult parsed)
        {
            return new ParseResponse(parsed, _addHosts);
        }

        private readonly IAdditionalHosts _addHosts;
    }
}