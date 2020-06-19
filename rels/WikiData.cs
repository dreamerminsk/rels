using HtmlAgilityPack;
using rels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rels
{
    public class WikiData
    {
        private static string DATA_REF = "https://www.wikidata.org/wiki/Special:EntityData/{0}";

        private static HtmlWeb web = new HtmlWeb();

        public static async Task<Person> GetPersonAsync(string wikiDataId)
        {
            var p = new Person();
            var page = await web.LoadFromWebAsync(string.Format(DATA_REF, wikiDataId));

            return p;
        }
    }
}
