using System;
using System.IO;
using System.Net;

namespace UpdateChecker.Lib
{
    public class Checker
    {
        string filePath;
        FileHandler fileHandler;

        public Checker()
        {
            filePath = "database.data";
            fileHandler = new FileHandler();
        }

        public string Url { get; set; }
        
        public bool Check(string url)
        {
            string page = DownloadPage(url);
            Page newPage = new Page(DateTime.Now, page);
            
            bool arePagesIdentical = ComparePages(newPage);

            fileHandler.Save(filePath, newPage);

            return arePagesIdentical;
        }

        private string DownloadPage(string url)
        {
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            using (Stream data = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private bool ComparePages(Page page)
        {
            Page lastPage = (Page)fileHandler.Read(filePath);
            if (lastPage == null)
            {
                fileHandler.Save(filePath, page);
                return false;
            }
            else
            {
                return lastPage.PageHtml == page.PageHtml;
            }
        }
    }
}