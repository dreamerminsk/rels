using HtmlAgilityPack;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rels.Wiki
{
    public class WikiMedia
    {
        private static string DATA_REF = "https://commons.wikimedia.org/wiki/File:{0}";

        private static HtmlWeb web = new HtmlWeb();

        static readonly HttpClient client = new HttpClient();


        public static async Task<Image> GetMediaAsync(string fileId)
        {
            var page = await web.LoadFromWebAsync(string.Format(DATA_REF, fileId));
            var rows = page.DocumentNode.SelectSingleNode("//div[@class='fullMedia']/p/a");
            if (rows != null)
            {
                return await GetImageAsync(rows.Attributes["href"]?.Value);
            }
            return null;
        }

        public static async Task<Image> GetImageAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return Image.FromStream(await response.Content.ReadAsStreamAsync());
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(url + "\r\n" + e.Message, e.GetType().Name);
                return null;
            }
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
