using m3uParser;
using m3uParser.Model;
using System.Collections.Generic;
using System.Linq;

namespace WinMediaBox.Classes
{
    public class M3U8Playlist : List<M3U8Item>
    {
        public M3U8Playlist(string path)
        {
            var parseRes = Parse(path);
            InitList(parseRes);
        }

        private Extm3u Parse(string path)
        {
            Extm3u simpleVodM3u = M3U.ParseFromFile(path);
            return simpleVodM3u;
        }

        private void InitList(Extm3u m3u)
        {
            for (var i = 0; i < m3u.Medias.Count(); i++)
            {
                M3U8Item item = new M3U8Item();
                item.url = $"http{m3u.Warnings.ElementAt(i).Split("http")[1]}";
                item.title = m3u.Medias.ElementAt(i).Title.RawTitle;
                item.number = i + 1;
                Add(item);
            }            
        }
    }

    public class M3U8Item
    {
        public string title;
        public string url;
        public int number;
    }
}
