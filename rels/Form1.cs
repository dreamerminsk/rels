using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string next = "Alexandra Feodorovna (Alix of Hesse)";

        private Stack<string> nexts = new Stack<string>();

        private async void Form1_Load(object sender, EventArgs e)
        {
            nexts.Push("Alexandra Feodorovna (Alix of Hesse)");
            HtmlWeb web = new HtmlWeb();
            while (nexts.Count > 0)
            {
                var page = await web.LoadFromWebAsync("https://en.wikipedia.org/wiki/" + nexts.Pop());
                var rows = page.DocumentNode.SelectNodes("//table[@class='infobox vcard']/tbody/tr");
                rows.Where(r => Filter(r, "Father")).Select(r => Map(r))
                    .Where(r => r != null).ToList()
                    .ForEach(t =>
                    {
                        richTextBox1.AppendText(t.Attributes["title"].Value + string.Format("{0}", nexts.Count) + "\r\n");
                        if (!string.IsNullOrEmpty(t.Attributes["title"].Value))
                        {
                            nexts.Push(t.Attributes["title"].Value);
                        }
                    });
                rows.Where(r => Filter(r, "Mother")).Select(r => Map(r))
                    .Where(r => r != null).ToList()
                    .ForEach(t =>
                    {
                        richTextBox1.AppendText(t.Attributes["title"].Value + string.Format("{0}", nexts.Count) + "\r\n");
                        if (!string.IsNullOrEmpty(t.Attributes["title"].Value))
                        {
                            nexts.Push(t.Attributes["title"].Value);
                        }
                    });
            }
        }

        private HtmlNode Map(HtmlNode node)
        {
            var children = node.SelectNodes("td/a[@href]");
            if (children != null)
            {
                return children.First();
            }
            return null;
        }

        private bool Filter(HtmlNode node, string text)
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
