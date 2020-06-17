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

        public Queue<string> q = new Queue<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe(x => ProcessPerson());
            q.Enqueue("Alexei Nikolaevich, Tsarevich of Russia");
            q.Enqueue("Elizabeth II");
            q.Enqueue("Margrethe II");
            q.Enqueue("Carl XVI Gustaf");
            q.Enqueue("Harald V");
            listBox1.DataSource = q.ToList();
            
        }

        private async void ProcessPerson()
        {
            var title = q.Dequeue();
            if (!string.IsNullOrEmpty(title))
            {
                richTextBox1.AppendText(string.Format("{0}\r\n", title));
                var infoBox = await Wiki.GetInfoBoxAsync(title);
                infoBox.ToList().ForEach(v=> {
                    if (v.Key.Equals("Father") || v.Key.Equals("Mother")) {
                        richTextBox1.AppendText(string.Format("\t{0}:\t{1}\r\n", v.Key, v.Value));
                        q.Enqueue(v.Value);
                        listBox1.BeginUpdate();
                        listBox1.DataSource = q.ToList();
                        listBox1.EndUpdate();
                    }
                    if (v.Key.Equals("Born") || v.Key.Equals("Died"))
                    {
                        richTextBox1.AppendText(string.Format("\t{0}:\t\t{1}\r\n", v.Key, v.Value));
                    }
                });
            }
        }
    }
}
