using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rels
{
    public class States
    {

        private static readonly string STATES_PAGE = "https://en.wikipedia.org/wiki/List_of_sovereign_states";

        static readonly HttpClient client = new HttpClient();

        private static readonly HtmlWeb htmlWeb = new HtmlWeb();

        [STAThread]
        static async Task Main()
        {
            var page = await htmlWeb.LoadFromWebAsync(STATES_PAGE);
            var refs = page.DocumentNode.SelectNodes("//div[@id='mw-content-text']//a[contains(@href, '/wiki/')]");
            refs.ToList().ForEach(r => Process(r));
        }

        private static void Process(HtmlNode node)
        {
            Console.WriteLine("---------------------------\r\n");
            Console.WriteLine(string.Format("{0}\r\n\t{1}", node.InnerText, node.Attributes["href"].Value));
            var pageRef = string.Format("https://en.wikipedia.org{0}", node.Attributes["href"].Value);
            if (pageRef.Contains("/wiki/File:")) return;
            if (pageRef.Contains("/wiki/Category:")) return;
            if (pageRef.Contains("/wiki/Template:")) return;
            if (pageRef.Contains("/wiki/Template_talk:")) return;
            var page = htmlWeb.Load(pageRef);
            var cats = page.DocumentNode.SelectNodes("//div[@id='mw-normal-catlinks']/ul/li/a");
            var wdRef = page.DocumentNode.SelectSingleNode("//li[@id='t-wikibase']/a[@href]");
            cats?.ToList()?.ForEach(cat => Console.WriteLine("\t" + cat.InnerText));
            Console.WriteLine(wdRef?.Attributes["href"]?.Value);
            Thread.Sleep(4);
        }

        static async Task<string> GetStringAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message, e.GetType().Name);
                return null;
            }
        }
    }
}
