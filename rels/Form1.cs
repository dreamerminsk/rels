using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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

        private Queue<string> q = new Queue<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            Observable.Interval(TimeSpan.FromSeconds(5)).Subscribe(x => { ProcessPerson(); });
            q.Enqueue("Alexei Nikolaevich, Tsarevich of Russia");
            q.Enqueue("Elizabeth II");
            q.Enqueue("Margrethe II");
            q.Enqueue("Carl XVI Gustaf");
            q.Enqueue("Harald V");
        }

        private async void ProcessPerson()
        {
            HtmlWeb web = new HtmlWeb();
            var page = await web.LoadFromWebAsync("https://en.wikipedia.org/wiki/" + q.Dequeue());
            var rows = page.DocumentNode.SelectNodes("//table[@class='infobox vcard']/tbody/tr");
            var fathers = rows?.Where(r => Filter(r, "Father"));
            if (fathers != null)
            {
                fathers.Select(r => Map(r))
                    .Where(r => r != null).ToList()
                    .ForEach(t =>
                    {
                        richTextBox1.AppendText(t.Attributes["title"].Value + string.Format(" - {0}", q.Count) + "\r\n");
                        if (!string.IsNullOrEmpty(t.Attributes["title"].Value))
                        {
                            //nexts.Push(t.Attributes["title"].Value);
                            q.Enqueue(t.Attributes["title"].Value);
                        }
                    });
            }
            var mothers = rows?.Where(r => Filter(r, "Mother"));
            if (mothers != null)
            {
                mothers.Select(r => Map(r))
                    .Where(r => r != null).ToList()
                    .ForEach(t =>
                    {
                        richTextBox1.AppendText(t.Attributes["title"].Value + string.Format(" - {0}", q.Count) + "\r\n");
                        if (!string.IsNullOrEmpty(t.Attributes["title"].Value))
                        {
                            //nexts.Push(t.Attributes["title"].Value);
                            q.Enqueue(t.Attributes["title"].Value);
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
