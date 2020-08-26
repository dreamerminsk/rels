﻿using HtmlAgilityPack;
using RateLimiter;
using System;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using ComposableAsync;

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
            await timeConstraint;
            var page = await htmlWeb.LoadFromWebAsync("https://www.tut.by/");
            var newsEntries = page.DocumentNode.SelectNodes("//div[contains(@class, 'news-entry')]");
            newsEntries.ToList().ForEach(newsEntry =>
            {
                richTextBox1.AppendText("ID: " + (newsEntry.Attributes["data-id"]?.Value ?? "-1") + "\r\n");
                var spanTitle = newsEntry.SelectSingleNode("a/span/span[contains(@class, '_title')]");
                if (spanTitle != null)
                {
                    richTextBox1.AppendText("TITLE: " + HttpUtility.HtmlDecode(spanTitle.InnerText) + "\r\n");
                }
                richTextBox1.AppendText("PUBLISHED: " +
                    UnixTimeStampToDateTime(double.Parse(newsEntry.Attributes["data-tm"]?.Value ?? "0")).ToString() + "\r\n");
                richTextBox1.AppendText("UPDATED: " +
                    UnixTimeStampToDateTime(double.Parse(newsEntry.Attributes["data-update-tm"]?.Value ?? "0")).ToString() + "\r\n");
                var spanComments = newsEntry.SelectSingleNode("a/span[@class='entry-cnt']/span[@class='entry-meta']/span[@class='entry-count']");
                if (spanComments != null)
                {
                    richTextBox1.AppendText("COMMENTS: " + spanComments.InnerText + "\r\n");
                }
                richTextBox1.AppendText(newsEntry.InnerHtml + "\r\n" + new string('-', 100) + "\r\n");
            });
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
            Enumerable.Range(1, 17).Select(x => 2008 - x).ToList().ForEach(async x =>
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
                var ht = row.SelectNodes("tbody/tr/th[@itemprop='homeTeam']");
                ht.ToList().ForEach(h =>
                {
                    richTextBox1.AppendText(string.Format("{0} - ", h?.InnerText.Trim()));
                });
                var at = row.SelectNodes("tbody/tr/th[@itemprop='awayTeam']");
                at.ToList().ForEach(h =>
                {
                    richTextBox1.AppendText(string.Format("{0} - ", h?.InnerText));
                });
                ht = row.SelectNodes("tbody/tr/th[@class='fscore']");
                ht.ToList().ForEach(h =>
                {
                    richTextBox1.AppendText(string.Format("{0}\r\n", h?.InnerText.Trim()));
                });
            });
        }
    }
}
