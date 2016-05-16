using System.Collections.Generic;
using System.Linq;
using DnsRip.MVC.Interfaces;

namespace DnsRip.MVC.Utilities
{
    public class AdditionalHosts : IAdditionalHosts
    {
        public IEnumerable<string> Find(string input)
        {
            var subdomains = new[]
            {
                "www",
                "m",
                "blog",
                "ftp",
                "imap",
                "pop",
                "smtp",
                "mail",
                "webmail"
            };

            var parts = input.Split('.');
            var firstPart = parts.First();
            var isSubdomain = subdomains.Any(s => s == firstPart);

            if (!isSubdomain)
                return subdomains.Select(s => s + "." + input).ToList();

            var root = string.Join(".", parts, 1, parts.Length - 1);

            var additional = new List<string>
            {
                root
            };

            additional.AddRange(subdomains.Where(s => s != firstPart).Select(s => s + "." + root));

            return additional;
        }
    }
}