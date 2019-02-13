using RssReader.Classes;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace RssReader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[,] sources =
            {
                { "http://www.interfax.by/news/feed", "Интерфакс" },
                { "http://habrahabr.ru/rss", "Хабрахабр" }
            };
            string xml;
            int readedNews = 0;
            int addedNews = 0;
            try
            {
                using (NewsContext newsContext = new NewsContext())
                {
                    using (SourceContext sourceContext = new SourceContext())
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            XmlDocument rssXmlDoc = new XmlDocument();
                            using (var webClient = new WebClient())
                            {
                                webClient.Encoding = Encoding.UTF8;
                                xml = webClient.DownloadString(sources[i, 0]).TrimStart();
                            }
                            StringReader reader = new StringReader(xml);
                            XmlReader xmlReader = XmlReader.Create(reader);
                            rssXmlDoc.Load(xmlReader);

                            string sourceUrl = rssXmlDoc.SelectSingleNode("rss/channel/image/link").InnerText;
                            string sourceName = sources[i, 1];

                            if (!sourceContext.Sources.Any(s => s.Url == sourceUrl))
                            {
                                Source source = new Source { Url = sourceUrl, Name = sourceName };
                                sourceContext.Sources.Add(source);
                                sourceContext.SaveChanges();
                            }
                            int sourceId = (from s in sourceContext.Sources where s.Url.Equals(sourceUrl) select s.SourceId).SingleOrDefault();

                            XmlNodeList newsItems = rssXmlDoc.SelectNodes("rss/channel/item");
                            foreach (XmlNode item in newsItems)
                            {
                                readedNews++;
                                string title = item.SelectSingleNode("title").InnerText;
                                DateTime publicationDate = DateTime.Parse(item.SelectSingleNode("pubDate").InnerText);
                                string description = Regex.Replace(item.SelectSingleNode("description").InnerText, "<.*?(>|$)", "");//убираем html
                                string newsUrl = item.SelectSingleNode("guid").InnerText;

                                if (!((from n in newsContext.News where n.Title.Equals(title) && n.PublicationDate == publicationDate select n.NewsId).SingleOrDefault() > 0))
                                {
                                    addedNews++;
                                    News news = new News { Title = title, PublicationDate = Convert.ToDateTime(publicationDate), Discription = description, NewsUrl = newsUrl, SourceId = sourceId };
                                    newsContext.News.Add(news);
                                    newsContext.SaveChanges();
                                }

                            }
                            Console.WriteLine("{0}:\r\n readed:{1}, added:{2}", sourceName, readedNews, addedNews);
                            addedNews = 0;
                            readedNews = 0;
                        }
                    }
                    Console.WriteLine("all news in db:{0}", newsContext.News.Count());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
