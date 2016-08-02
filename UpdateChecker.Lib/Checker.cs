using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace UpdateChecker.Lib
{
    public enum Status
    {
        FIRST_QUERY,        // first request to app
        NO_CHANGES,         // both sites are identical
        CHANGED,            // site has changed
        DIFFERENT,          // different site was requested
        CONNECTION_ERROR,   // Internet error
        FILE_ERROR          // file r/w permission error
    }

    public class Checker
    {
        string filePath;
        public string Url { get; set; }

        public Checker()
        {
            filePath = "database.data";
        }
        
        public async Task<Status> Check(string url)
        {
            string page = await DownloadPage(url);
            if (page == null)
                return Status.CONNECTION_ERROR;
            
            Page newPage = new Page(DateTime.Now, url, page);

            Status arePagesIdentical = ComparePages(newPage);
            
            FileHandler.Save(filePath, newPage);

            return arePagesIdentical;
        }

        private async Task<string> DownloadPage(string url)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                using (Stream data = await client.OpenReadTaskAsync(url))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                return null;
            }
        }

        private Status ComparePages(Page page)
        {
            Page lastPage = (Page)FileHandler.Read(filePath);

            if (lastPage == null)
            {
                return Status.FIRST_QUERY;
            }
            else if (page.Url != lastPage.Url)
            {
                return Status.DIFFERENT;
            }
            else if (page.PageHtml != lastPage.PageHtml)
            {
                return Status.CHANGED;
            }
            else
            {
                return Status.NO_CHANGES;
            }
        }
    }
}