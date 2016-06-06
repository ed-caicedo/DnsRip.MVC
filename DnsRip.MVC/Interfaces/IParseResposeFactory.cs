using DnsRip.MVC.Requests;
using DnsRip.MVC.Responses;

namespace DnsRip.MVC.Interfaces
{
    public interface IParseResponseFactory
    {
        ParseResponse Create(ParseRequest request);
    }
}