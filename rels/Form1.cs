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

        private HashSet<string> processed = new HashSet<string>();

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
                AppendText(string.Format("{0}\r\n", title));
                var infoBox = await Wiki.GetInfoBoxAsync(title);
                infoBox.ToList().ForEach(v =>
                {
                    if (v.Key.Equals("Father") || v.Key.Equals("Mother"))
                    {
                        AppendText(string.Format("\t{0}:\t{1}\r\n", v.Key, v.Value));
                        if (!processed.Contains(v.Value))
                        {
                            q.Enqueue(v.Value);
                        }
                        UpdateQueue();
                    }
                    if (v.Key.Equals("Born") || v.Key.Equals("Died"))
                    {
                        AppendText(string.Format("\t{0}:\t\t{1}\r\n", v.Key, v.Value));
                    }
                });
            }
        }

        private void AppendText(string text)
        {
            richTextBox1.Invoke(new Action(() => { richTextBox1.AppendText(text); }));
        }

        private void UpdateQueue()
        {
            var si = listBox1.SelectedIndex;
            listBox1.BeginUpdate();
            listBox1.DataSource = q.ToList();
            listBox1.SelectedIndex = si;
            listBox1.EndUpdate();
            SetTitle("QUEUE / " + q.Count + " /");
        }

        private void SetTitle(string text)
        {
            Form1.ActiveForm.Invoke(new Action(() => { Form1.ActiveForm.Text = text; }));
        }
    }
}
