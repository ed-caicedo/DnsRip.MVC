using DnsRip.Models;
using DnsRip.MVC.Models;

namespace DnsRip.MVC.Interfaces
{
    public interface IParseResposeFactory
    {
        IParseResponse Create(ParseResult parsed);
    }
}