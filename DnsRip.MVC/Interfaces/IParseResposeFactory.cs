using DnsRip.Models;
using DnsRip.MVC.Requests;

namespace DnsRip.MVC.Interfaces
{
    public interface IParseResponseFactory
    {
        IParseResponse Create(ParseRequest request);
    }
}