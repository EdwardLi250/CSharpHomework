using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCrawler
{
    public delegate void DownloadHandler(string obj);
    class SimpleCrawler
    {
        private Hashtable urls;
        private int count;
        public event DownloadHandler PageDownloaded;
        
        public SimpleCrawler()
        {
            urls = new Hashtable();
            count = 0;
        }

        public void AddUrl(string url)
        {
            urls.Clear();
            urls.Add(url, false);
        }

        public void Crawl()
        {
            //Console.WriteLine("开始爬行了.... ");
            PageDownloaded("开始爬行了...");
            while (true)
            {
                string current = null;
                foreach (string url in urls.Keys)
                {
                    if ((bool)urls[url]) continue;
                    current = url;
                }

                if (current == null || count > 10) break;;
                PageDownloaded("爬行" + current + "页面!");
                string html = DownLoad(current); // 下载
                urls[current] = true;
                count++;

                Parse(html, current);//解析,并加入新的链接
                PageDownloaded("爬行结束");
            }
        }

        public string DownLoad(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);
                string fileName = count.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
                return html;
            }
            catch (Exception ex)
            {
                PageDownloaded(ex.Message);
                return "";
            }
        }

        private void Parse(string html, string current)
        {
            string strRef = @"(href|HREF)[]*=[]*[""'][^""'#>]+[""']";
            MatchCollection matches = new Regex(strRef).Matches(html);
            string strRef1 = @"https://.+[.]com/";
            string strRef2 = @"(/.+[.]html$|/$)";
            Regex regex1 = new Regex(strRef1);
            Regex regex2 = new Regex(strRef2);
            string temp = current;
            foreach (Match match in matches)
            {
                strRef = match.Value.Substring(match.Value.IndexOf('=') + 1)
                          .Trim('"', '\"', '#', '>');
                if (!Regex.IsMatch(strRef, @"^https:"))
                {
                    if (!Regex.IsMatch(strRef, @"^/"))
                    {
                        temp = regex1.Match(temp).ToString();
                        strRef = temp + strRef;
                    }
                    else
                    {
                        temp = regex2.Replace(temp, "");
                        strRef = temp + strRef;
                    }
                }
                if (!Regex.IsMatch(strRef, regex1.Match(current).ToString()) || !Regex.IsMatch(strRef, @"html$")) continue;
                if (strRef.Length == 0) continue;
                if (urls[strRef] == null) urls[strRef] = false;
            }
        }
    }
}
