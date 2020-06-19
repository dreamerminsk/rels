using HtmlAgilityPack;
using LinqToDB.Common;
using Newtonsoft.Json.Linq;
using rels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rels
{
    public class WikiData
    {
        private static string DATA_REF = "https://www.wikidata.org/wiki/Special:EntityData/{0}.json";

        private static HtmlWeb web = new HtmlWeb();

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
            p.Name = p.Name.IsNullOrEmpty() ? p.Name: "en:???";
            p.RusName = labels["ru"]?["value"]?.ToString();
            p.DateOfBirth = claims["P569"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["time"].ToString();
            p.DateOfDeath = claims["P570"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["time"].ToString();
            p.Father = claims["P22"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"].ToString();
            p.Mother = claims["P25"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"].ToString();
            return p;
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
