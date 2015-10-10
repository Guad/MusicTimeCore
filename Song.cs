using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace MusicTimeCore
{
    public class Song
    {
        public string Name { get; set; } 
        public string Artist { get; set; }
        public string Uri { get; set; }

        public SongInfo AdditionalInfo
        {
            get
            {
                if (internalInfo == null)
                {
                    internalInfo = FetchSongInfo();
                }
                return internalInfo;
            }
        }

        private SongInfo internalInfo;

        private SongInfo FetchSongInfo()
        {
            //https://api.spotify.com/v1/search?q=a%20stranger%20i%20remain&type=track
            UriBuilder uriBuilder = new UriBuilder("https://api.spotify.com/v1/search");
            var parser = HttpUtility.ParseQueryString(string.Empty);
            parser["q"] = this.Name;
            parser["type"] = "track";
            uriBuilder.Query = parser.ToString();
            string rawJson;
            using (WebClient client = new WebClient())
            {
                rawJson = client.DownloadString(uriBuilder.Uri);
            }
            var serializer = new JsonSerializer();
            var json = serializer.Deserialize<SongInfoApi>(new JsonTextReader(new StringReader(rawJson)));
            var tmpSongInfo = new SongInfo();
            if (json.tracks.items.Count == 0) return null;
            tmpSongInfo.Album = json.tracks.items[0].album.name;
            tmpSongInfo.CoverUri64 = json.tracks.items[0].album.images.First(i => i.width == 64).url;
            tmpSongInfo.CoverUri300 = json.tracks.items[0].album.images.First(i => i.width == 300).url;
            tmpSongInfo.CoverUri600 = json.tracks.items[0].album.images.First(i => i.width == 600).url;
            return tmpSongInfo;
        }
    }

    public class SongInfo
    {
        public string CoverUri600 { get; set; }
        public string CoverUri300 { get; set; }
        public string CoverUri64 { get; set; }
        public string Album { get; set; }
    }

    public class ExternalUrls
    {
        public string spotify { get; set; }
    }

    public class Image
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Album
    {
        public string album_type { get; set; }
        public List<string> available_markets { get; set; }
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class ExternalUrls2
    {
        public string spotify { get; set; }
    }

    public class Artist
    {
        public ExternalUrls2 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class ExternalIds
    {
        public string isrc { get; set; }
    }

    public class ExternalUrls3
    {
        public string spotify { get; set; }
    }

    public class Item
    {
        public Album album { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool @explicit { get; set; }
        public ExternalIds external_ids { get; set; }
        public ExternalUrls3 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Tracks
    {
        public string href { get; set; }
        public List<Item> items { get; set; }
        public int limit { get; set; }
        public object next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class SongInfoApi
    {
        public Tracks tracks { get; set; }
    }
}