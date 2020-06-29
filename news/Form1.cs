using HtmlAgilityPack;
using System;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace news
{
    public partial class Form1 : Form
    {

        private HtmlWeb htmlWeb = new HtmlWeb();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
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
    }
}
