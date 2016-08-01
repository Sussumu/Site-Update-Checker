using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateChecker.Lib
{
    [Serializable]
    class Page
    {
        public Page(DateTime date, string pageHtml)
        {
            Date = date;
            PageHtml = pageHtml;
        }

        public DateTime Date { get; set; }
        public string PageHtml { get; set; }
    }
}
