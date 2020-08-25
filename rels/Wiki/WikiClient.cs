using HtmlAgilityPack;
using rels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rels.Wiki
{
    public class WikiClient
    {

        private static HtmlWeb web = new HtmlWeb();

        public static async Task KnockoutStageAsync()
        {
            string clref = "https://en.wikipedia.org/wiki/2011–12_UEFA_Champions_League_knockout_phase";
            var page = await web.LoadFromWebAsync(clref);
            var rows = page.DocumentNode.SelectNodes("//table[@class='fevent']");
            rows?.ToList().ForEach(row =>
            {
                var ht = row.SelectNodes("tbody/tr/th[@itemprop='homeTeam']");
                ht.ToList().ForEach(h=>
                {
                    Console.WriteLine(h?.InnerText);
                });
                var at = row.SelectNodes("tbody/tr/th[@itemprop='awayTeam']");
                at.ToList().ForEach(h =>
                {
                    Console.WriteLine(h?.InnerText);
                });
                ht = row.SelectNodes("tbody/tr/th[@class='fscore']");
                ht.ToList().ForEach(h =>
                {
                    Console.WriteLine(h?.InnerText);
                });
            });
        }

        public static async Task<Dictionary<string, string>> GetInfoBoxAsync(string title)
        {
            var infoBox = new Dictionary<string, string>();
            var page = await web.LoadFromWebAsync("https://en.wikipedia.org/wiki/" + title);
            var rows = page.DocumentNode.SelectNodes("//table[@class='infobox vcard']/tbody/tr");
            rows?.Where(r => Filter(r, "Father"))?.Select(r => ToRef(r))?.Where(r => r != null)?.ToList()
                    .ForEach(t =>
                    {
                        infoBox.Add("Father", t.Attributes["title"]?.Value);
                    });

            rows?.Where(r => Filter(r, "Mother"))?.Select(r => ToRef(r))?.Where(r => r != null)?.ToList()
                    .ForEach(t =>
                    {
                        infoBox.Add("Mother", t.Attributes["title"]?.Value);
                    });

            rows?.Where(r => Filter(r, "Born"))?.Select(r => ToText(r))?.Where(r => r != null)?.ToList()
                    .ForEach(t =>
                    {
                        infoBox.Add("Born", t);
                    });

            rows?.Where(r => Filter(r, "Died"))?.Select(r => ToText(r))?.Where(r => r != null)?.ToList()
                    .ForEach(t =>
                    {
                        infoBox.Add("Died", t);
                    });

            return infoBox;
        }

        public static async Task<Human> GetPersonAsync(string title)
        {
            var p = new Human();
            var page = await web.LoadFromWebAsync("https://en.wikipedia.org/wiki/" + title);

            var rows = page.DocumentNode.SelectNodes("//table[@class='infobox vcard']/tbody/tr");
            rows?.Where(r => Filter(r, "Father"))?.Select(r => ToRef(r))?.Where(r => r != null)?.ToList()
                    .ForEach(t =>
                    {
                        p.Father = t.Attributes["title"]?.Value;
                    });

            rows?.Where(r => Filter(r, "Mother"))?.Select(r => ToRef(r))?.Where(r => r != null)?.ToList()
                    .ForEach(t =>
                    {
                        p.Mother = t.Attributes["title"]?.Value;
                    });

            return p;
        }

        private static HtmlNode ToRef(HtmlNode node)
        {
            var children = node.SelectNodes("td/a[@href]");
            if (children != null)
            {
                return children.First();
            }
            return null;
        }

        private static string ToText(HtmlNode node)
        {
            var children = node.SelectNodes("td");
            if (children != null)
            {
                return children.First()?.InnerText;
            }
            return null;
        }

        private static bool Filter(HtmlNode node, string text)
        {
            var ths = node.SelectNodes("th[@scope='row']");
            if (ths != null)
            {
                if (ths.First().InnerText.Equals(text))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
