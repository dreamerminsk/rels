using LinqToDB.Common;
using Newtonsoft.Json.Linq;
using rels.Model;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rels
{
    public class WikiData
    {
        private static string DATA_REF = "https://www.wikidata.org/wiki/Special:EntityData/{0}.json";

        static readonly HttpClient client = new HttpClient();

        public static async Task<Person> GetPersonAsync(string wikiDataId)
        {
            var p = new Person();
            var page = await GetStringAsync(string.Format(DATA_REF, wikiDataId));
            var doc = JObject.Parse(page);
            var claims = doc["entities"]?[wikiDataId]?["claims"];
            var labels = doc["entities"]?[wikiDataId]?["labels"];
            p.ID = int.Parse(wikiDataId.Substring(1));
            p.WikiDataID = wikiDataId;
            p.Name = labels["en"]?["value"]?.ToString();
            p.RusName = labels["ru"]?["value"]?.ToString();
            p.Country = claims["P27"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"].ToString();
            p.DateOfBirth = claims["P569"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["time"].ToString();
            p.DateOfDeath = claims["P570"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["time"].ToString();
            p.Father = claims["P22"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"].ToString();
            p.Mother = claims["P25"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"].ToString();
            return p;
        }

        public static async Task<Country> GetCountryAsync(string wikiDataId)
        {
            var c = new Country();
            if (wikiDataId.IsNullOrEmpty()) return c;
            var page = await GetStringAsync(string.Format(DATA_REF, wikiDataId));
            var doc = JObject.Parse(page);
            var claims = doc["entities"]?[wikiDataId]?["claims"];
            var labels = doc["entities"]?[wikiDataId]?["labels"];
            c.ID = int.Parse(wikiDataId.Substring(1));
            c.WikiDataID = wikiDataId;
            c.Name = labels["en"]?["value"]?.ToString();
            c.RusName = labels["ru"]?["value"]?.ToString();
            return c;
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
