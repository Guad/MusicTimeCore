using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace MusicTimeCore
{
    public static class SourceFetch
    {
        public enum Source
        {
            Mp3WithMe,
        }

        public static IEnumerable<Song> Search(string query)
        {
            return Search(query, Source.Mp3WithMe);
        }

        public static IEnumerable<Song> Search(string query, Source source)
        {
            if (String.IsNullOrWhiteSpace(query)) throw new ArgumentNullException("Argument " + nameof(query) + " is null or whitespace.");
            switch (source)
            {
                default:
                case Source.Mp3WithMe:
                    {
                        UriBuilder uriBuilder = new UriBuilder("http://mp3with.co/search/");
                        var parser = HttpUtility.ParseQueryString(string.Empty);
                        parser["q"] = query;
                        uriBuilder.Query = parser.ToString();
                        string rawHtml;
                        using (WebClient client = new WebClient()) 
                        {
                            rawHtml = client.DownloadString(uriBuilder.Uri);
                        }
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(rawHtml);
                        //List<Song> output = new List<Song>();
                        foreach (HtmlNode node in doc.DocumentNode.SelectSingleNode("//ul[@class=\"songs\"]").SelectNodes("li"))
                        {
                            yield return new Song() // TODO: Learn HTMLAgilityPack
                            {
                                Uri = "http://mp3with.co" + node.Attributes["data-mp3"].Value,
                                Name = node.ChildNodes[1].ChildNodes.First(n => n.Name == "strong" && !n.Attributes.Contains("class")).InnerHtml.Trim(),
                                Artist = node.ChildNodes[1].ChildNodes.First(n => n.GetAttributeValue("class", "none") == "artist").InnerHtml.Trim(),
                            };
                            // fetch album info here
                        }
                    }
                    break;
            }
        }

    }
}