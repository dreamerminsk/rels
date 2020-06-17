using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rels
{
    public class Wiki
    {

        private static HtmlWeb web = new HtmlWeb();

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
