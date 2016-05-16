using System.Collections.Generic;

namespace DnsRip.MVC.Interfaces
{
    public interface IAdditionalHosts
    {
        IEnumerable<string> Find(string input);
    }
}