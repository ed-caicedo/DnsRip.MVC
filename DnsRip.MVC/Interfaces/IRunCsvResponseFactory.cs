using DnsRip.MVC.Requests;

namespace DnsRip.MVC.Interfaces
{
    public interface IRunCsvResponseFactory
    {
        IRunCsvReponseStream Create(RunRequest request);
    }
}