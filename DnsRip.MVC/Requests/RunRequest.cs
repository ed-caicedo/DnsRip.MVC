namespace DnsRip.MVC.Requests
{
    public class RunRequest
    {
        public string[] Domains { get; set; }
        public string[] Types { get; set; }
        public string Server { get; set; }
    }
}