using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace rels.Wiki
{
    public static class Web
    {
        static readonly HttpClient client = new HttpClient();
        private static Subject<string> log = new Subject<string>();
        private static WebStats stringStats = new WebStats { Name= "GetStringAsync" };
        private static Subject<WebStats> stats = new Subject<WebStats>();

        public static IObservable<string> Log =>
            log.AsObservable();

        public static IObservable<WebStats> Stats =>
            stats.AsObservable();

        public static async Task<string> GetStringAsync(string url)
        {
            try
            {
                stringStats.Requests += 1;
                log.OnNext(string.Format("{0} - GetStringAsync - {1}\r\n", DateTime.Now.ToLongTimeString(), url));
                HttpResponseMessage response = await client.GetAsync(url);
                log.OnNext(string.Format("{0} - {1} - {2}\r\n", DateTime.Now, (int)response.StatusCode, response.ReasonPhrase));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                stringStats.Bytes += ASCIIEncoding.UTF8.GetByteCount(responseBody);
                stats.OnNext(stringStats);
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                log.OnNext(string.Format("{0} - {1} - {2}\r\n", DateTime.Now, e.GetType().Name, e.Message));
                return null;
            }
        }

        public static async Task<string> PostStringAsync(string url, string content)
        {
            try
            {
                stringStats.Requests += 1;
                log.OnNext(string.Format("{0} - PostStringAsync - {1}\r\n", DateTime.Now.ToLongTimeString(), url));
                HttpResponseMessage response = await client.PostAsync(url, new StringContent(content));
                log.OnNext(string.Format("{0} - {1} - {2}\r\n", DateTime.Now, (int)response.StatusCode, response.ReasonPhrase));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                stringStats.Bytes += ASCIIEncoding.UTF8.GetByteCount(responseBody);
                stats.OnNext(stringStats);
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                log.OnNext(string.Format("{0} - {1} - {2}\r\n", DateTime.Now, e.GetType().Name, e.Message));
                return null;
            }
        }

        public class WebStats
        {
            public string Name { get; set; }

            public int Requests { get; set; } = 0;

            public long Bytes { get; set; } = 0;
        }
    }
}
