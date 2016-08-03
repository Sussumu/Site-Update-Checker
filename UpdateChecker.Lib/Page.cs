using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateChecker.Lib
{
    [Serializable]
    public class Page
    {
        public Page(DateTime date, string url, string pageHtml)
        {
            Date = date;
            Url = url;
            PageHtml = pageHtml;
        }

        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string PageHtml { get; set; }
    }
}
