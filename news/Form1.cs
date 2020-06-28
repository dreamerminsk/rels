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
                var spanTitle = newsEntry.SelectSingleNode("a/span/span[contains(@class, '_title')]");
                if (spanTitle != null)
                {
                    richTextBox1.AppendText(HttpUtility.HtmlDecode(spanTitle.InnerText) + "\r\n");
                }                
                richTextBox1.AppendText(newsEntry.InnerHtml + "\r\n" + new string('-', 100) + "\r\n");
            });
        }
    }
}
