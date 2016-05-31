using FileHelpers;

namespace DnsRip.MVC.Responses
{
    [DelimitedRecord(",")]
    public class RunCsvResponse
    {
        public string Query { get; set; }
        public string Type { get; set; }
        public string Result { get; set; }
    }
}