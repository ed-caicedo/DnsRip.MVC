using System.Collections.Generic;
using System.Linq;

namespace DnsRip.MVC.Models
{
    public class RunResponseViewModel
    {
        public string Query { get; set; }
        public IEnumerable<RunRecordModel> Records { get; set; }
        public bool IsValid { get; set; }
        public string Error { get; set; }
        
    }
}