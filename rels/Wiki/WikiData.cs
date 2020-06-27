using LinqToDB.Common;
using Newtonsoft.Json.Linq;
using rels.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rels.Wiki
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
            var descriptions = doc["entities"]?[wikiDataId]?["descriptions"];
            //if (claims == null)
            //{
            //    claims = doc["entities"]?[doc["entities"]?.First]?["claims"];
            //    labels = doc["entities"]?[doc["entities"]?.First]?["labels"];
            //}

            p.ID = int.Parse(wikiDataId.Substring(1));
            p.WikiDataID = wikiDataId;

            if (labels == null)
            {
                return p;
            }
            p.Name = labels["en"]?["value"]?.ToString();
            if (p.Name.IsNullOrEmpty() || p.Name.Equals("???"))
            {
                JProperty f = (JProperty)labels.Children().First();
                JObject v = (JObject)f.Value;
                p.Name = string.Format("{0}: {1}", f.Name, v.Value<string>("value"));
            }
            p.RusName = labels["ru"]?["value"]?.ToString();
            p.Description += descriptions["en"]?["value"]?.ToString();
            p.Description += "\r\n" + descriptions["ru"]?["value"]?.ToString();

            if (claims == null)
            {
                return p;
            }

            p.ImageFile = claims["P18"]?[0]?["mainsnak"]?["datavalue"]?["value"]?.ToString();
            p.Country = claims["P27"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"]?.ToString();
            p.DateOfBirth = claims["P569"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["time"]?.ToString();
            p.DateOfDeath = claims["P570"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["time"]?.ToString();
            p.Father = claims["P22"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"]?.ToString();
            p.Mother = claims["P25"]?[0]?["mainsnak"]?["datavalue"]?["value"]?["id"]?.ToString();
            p.Siblings = ParseSiblings(claims);
            p.Spouse = ParseSpouse(claims);
            p.Children = ParseChildren(claims);
            return p;
        }

        private static List<string> ParseSiblings(JToken claims)
        {
            var ss = new List<string>();
            var siblings = claims["P3373"];
            if (siblings?.HasValues ?? false)
            {
                foreach (var sibling in siblings)
                {
                    ss.Add(sibling["mainsnak"]?["datavalue"]?["value"]?["id"]?.ToString());
                }
            }
            return ss;
        }

        private static List<string> ParseSpouse(JToken claims)
        {
            var ss = new List<string>();
            var spouses = claims["P26"];
            if (spouses?.HasValues ?? false)
            {
                foreach (var spouse in spouses)
                {
                    ss.Add(spouse["mainsnak"]?["datavalue"]?["value"]?["id"]?.ToString());
                }
            }
            return ss;
        }

        private static List<string> ParseChildren(JToken claims)
        {
            var ss = new List<string>();
            var children = claims["P40"];
            if (children?.HasValues ?? false)
            {
                foreach (var child in children)
                {
                    ss.Add(child["mainsnak"]?["datavalue"]?["value"]?["id"]?.ToString());
                }
            }
            return ss;
        }


        public static async Task<Country> GetCountryAsync(string wikiDataId)
        {
            var c = new Country();
            if (wikiDataId.IsNullOrEmpty()) return c;
            var page = await GetStringAsync(string.Format(DATA_REF, wikiDataId));
            var doc = JObject.Parse(page);
            JProperty entities = (JProperty)doc["entities"].Children().First();
            var claims = entities?.Value?["claims"];
            var labels = entities?.Value?["labels"];
            c.ID = int.Parse(entities?.Name?.Substring(1));
            c.WikiDataID = entities?.Name;
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
