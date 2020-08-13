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

        public static IObservable<string> Log =>
            log.AsObservable();

        public static async Task<string> GetStringAsync(string url)
        {
            try
            {
                log.OnNext(string.Format("{0} - GetStringAsync - {1}\r\n", DateTime.Now, url));
                HttpResponseMessage response = await client.GetAsync(url);
                log.OnNext(string.Format("{0} - {1} - {2}\r\n", DateTime.Now, response.StatusCode, response.ReasonPhrase));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                log.OnNext(string.Format("{0} - {1} - {2}\r\n", DateTime.Now, e.GetType().Name, e.Message));
                return null;
            }
        }
    }
}
