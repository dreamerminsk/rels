using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rels.Wiki
{
    public class WebStats
    {
        public string Name { get; set; }

        public int Requests { get; set; } = 0;

        public long Bytes { get; set; } = 0;

        private static readonly SortedDictionary<string, WebStats> HOSTS = new SortedDictionary<string, WebStats>();

        public static WebStats GetHostStats(string url)
        {
            string host = GetHost(url);
            WebStats stats;
            if (!HOSTS.TryGetValue(host, out stats))
            {
                stats = new WebStats { Name = host, Requests = 0, Bytes = 0 };
                HOSTS.Add(host, stats);
            }
            return stats;
        }

        private static string GetHost(string url)
        {
            Uri NewUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out NewUri))
            {
                return NewUri.Authority;
            }
            return "localhost";
        }
    }
}
