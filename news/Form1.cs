using HtmlAgilityPack;
using RateLimiter;
using System;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using ComposableAsync;
using System.Collections.Generic;

namespace news
{
    public partial class Form1 : Form
    {

        private TimeLimiter timeConstraint = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(16));

        private HtmlWeb htmlWeb = new HtmlWeb();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enumerable.Range(1, 12).Select(x => 2020 - x).ToList().ForEach(async x =>
            {
                await timeConstraint;
                string url = string.Format(
                    "https://en.wikipedia.org/wiki/{0}–{1}_UEFA_Champions_League_knockout_phase",
                    x, (x + 1).ToString().Substring(2));
                await ParseMatches(url);
            });
            Enumerable.Range(1, 16).Select(x => 2008 - x).ToList().ForEach(async x =>
            {
                await timeConstraint;
                string url = string.Format(
                    "https://en.wikipedia.org/wiki/{0}–{1}_UEFA_Champions_League_knockout_stage",
                    x, (x + 1).ToString().Substring(2));
                await ParseMatches(url);
            });
        }

        private async System.Threading.Tasks.Task ParseMatches(string url)
        {
            var page = await htmlWeb.LoadFromWebAsync(url);
            var h1s = page.DocumentNode.SelectNodes("//h1[@class='firstHeading']");
            h1s?.ToList().ForEach(h1 =>
            {
                richTextBox1.AppendText(string.Format("{0}\r\n", url));
                richTextBox1.AppendText(string.Format("{0}\r\n", h1.InnerText));
            });
            var rows = page.DocumentNode.SelectNodes("//table[@class='fevent']");
            rows?.ToList().ForEach(row =>
            {
                var ht = row.SelectNodes("tbody/tr/th[@itemprop='homeTeam']/span/a");
                ht.ToList().ForEach(h =>
                {
                    richTextBox1.AppendText(string.Format("{0} - ", h?.Attributes["title"].Value));
                });
                var at = row.SelectNodes("tbody/tr/th[@itemprop='awayTeam']/span/a");
                at.ToList().ForEach(h =>
                {
                    richTextBox1.AppendText(string.Format("{0} - ", h?.Attributes["title"].Value));
                });
                ht = row.SelectNodes("tbody/tr/th[@class='fscore']");
                ht.ToList().ForEach(h =>
                {
                    richTextBox1.AppendText(string.Format("{0}\r\n", HttpUtility.HtmlDecode(h?.InnerText.Trim())));
                });
            });
        }

        private List<int> ExtractNumbers(string text)
        {
            List<int> numbers = new List<int>();
            string number = "0";
            foreach (char letter in text)
            {
                if (Char.IsDigit(letter))
                {
                    number += letter;
                } else
                {
                    numbers.Add(int.Parse(number));
                    number = "0";
                }
            }
            return numbers;
        }
    }
}
