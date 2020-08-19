using LinqToDB;
using LinqToDB.Common;
using rels.Model;
using rels.UI;
using rels.Wiki;
using rels.Workers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;

namespace rels
{
    public partial class Form1 : Form
    {

        private Updater updater = new Updater();

        public Form1()
        {
            InitializeComponent();
        }

        public List<string> q = new List<string>();

        private async void Form1_Load(object sender, EventArgs e)
        {
            Observable.Interval(TimeSpan.FromSeconds(16)).Subscribe(x => UpdateStats());
            button1.PerformClick();
            updater.Start();
            nameFlag.Image = await WikiFlags.GetEnglishAsync(18).ConfigureAwait(true);
            nameFlag.Width = nameFlag.Image.Width + 2;
            nameFlag.Height = nameFlag.Image.Height + 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new RelsDB())
            {
                var ps = db.GetTable<Human>();

                var bs = ps.GroupBy(p => p.DateOfBirth.Substring(0, 3));
                var births = new Dictionary<string, int>();
                bs.ForEachAsync(g => births.Add(g.Key ?? "UNKNOWN", g.Count()));

                var ds = ps.GroupBy(p => p.DateOfDeath.Substring(0, 3));
                var deaths = new Dictionary<string, int>();
                ds.ForEachAsync(g => deaths.Add(g.Key ?? "UNKNOWN", g.Count()));

                UpdateCents(births, deaths);
            }
        }

        private void UpdateCents(Dictionary<string, int> births, Dictionary<string, int> deaths)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => { SetCents(births, deaths); }));
            }
            else
            {
                SetCents(births, deaths);
            }
        }

        private void SetCents(Dictionary<string, int> births, Dictionary<string, int> deaths)
        {
            this.Text = Thread.CurrentThread.ManagedThreadId + "";
            listView1.BeginUpdate();

            if (listView1.Columns.Count != 3)
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();

                listView1.Columns.Add("Century", 80, HorizontalAlignment.Right);
                listView1.Columns.Add("Births", 80, HorizontalAlignment.Right);
                listView1.Columns.Add("Deaths", 80, HorizontalAlignment.Right);
            }

            births.ToList().ForEach(b =>
            {
                var lis = listView1.Items.Cast<ListViewItem>()
               .Where(x => (x.Text == b.Key))
               .FirstOrDefault();
                if (lis == null)
                {
                    var li = listView1.Items.Add(b.Key);
                    li.SubItems.Add(b.Value.ToString());
                    li.SubItems.Add("0");
                }
                else
                {
                    if (lis.SubItems[1].Text.Equals(b.Value.ToString()))
                    {
                        lis.BackColor = Color.White;
                        lis.SubItems[1].Font = new Font(lis.SubItems[1].Font, FontStyle.Regular);
                    }
                    else
                    {
                        lis.BackColor = Color.Aqua;
                        lis.SubItems[1].Text = b.Value.ToString();
                        lis.SubItems[1].Font = new Font(lis.SubItems[1].Font, FontStyle.Bold);
                    }
                }
            });

            deaths.ToList().ForEach(d =>
            {
                var lis = listView1.Items.Cast<ListViewItem>()
               .Where(x => (x.Text == d.Key))
               .FirstOrDefault();
                if (lis == null)
                {
                    var li = listView1.Items.Add(d.Key);
                    li.SubItems.Add("0");
                    li.SubItems.Add(d.Value.ToString());
                }
                else
                {
                    if (lis.SubItems[2].Text.Equals(d.Value.ToString()))
                    {
                        //lis.BackColor = Color.White;
                        lis.SubItems[2].Font = new Font(lis.SubItems[2].Font, FontStyle.Regular);
                    }
                    else
                    {
                        lis.BackColor = Color.Aqua;
                        lis.SubItems[2].Text = d.Value.ToString();
                        lis.SubItems[2].Font = new Font(lis.SubItems[2].Font, FontStyle.Bold);
                    }
                    lis.SubItems[2].Text = d.Value.ToString();
                }
            });

            listView1.EndUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //using (var db = new RelsDB())
            //{
            //var ps = db.GetTable<Human>();
            //var bs = ps.GroupBy(p => p.Country);
            //var cs = new Dictionary<string, int>();
            //var c = await Countries.GetByWikiDataIdAsync(g.Key);
            //bs.ForEachAsync(g => cs.Add(c?.Name ?? "UNKNOWN", g.Count()));
            //UpdateCountries(cs);
            //}
        }

        private void UpdateCountries(Dictionary<string, int> countries)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => { SetCountries(countries); }));
            }
            else
            {
                SetCountries(countries);
            }
        }

        private void SetCountries(Dictionary<string, int> countries)
        {
            listView1.BeginUpdate();

            if (listView1.Columns.Count != 2)
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();

                listView1.Columns.Add("Country", 200, HorizontalAlignment.Left);
                listView1.Columns.Add("Count", 80, HorizontalAlignment.Right);
            }

            countries.ToList().ForEach(c =>
            {
                var lis = listView1.Items.Cast<ListViewItem>()
               .Where(x => (x.Text == c.Key))
               .FirstOrDefault();
                if (lis == null)
                {
                    var li = listView1.Items.Add(c.Key);
                    li.SubItems.Add(c.Value.ToString());
                }
                else
                {
                    if (lis.SubItems[1].Text.Equals(c.Value.ToString()))
                    {
                        lis.BackColor = Color.White;
                    }
                    else
                    {
                        lis.BackColor = Color.Aqua;
                        lis.SubItems[1].Text = c.Value.ToString();
                    }
                }
            });

            listView1.EndUpdate();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListViewItemComparer sorter = new ListViewItemComparer(e.Column);
            if (e.Column > 0)
            {
                sorter.Numeric = true;
            }
            listView1.ListViewItemSorter = sorter;
            listView1.Sort();
        }

        private void UpdateStats()
        {
            if (listView1.Columns.Count > 0)
            {
                if (listView1.Columns[0].Text == "Century")
                {
                    button1.Invoke(new Action(() => button1.PerformClick()));
                }
                else
                {
                    button2.Invoke(new Action(() => button2.PerformClick()));
                }
            }
            else
            {
                button1.Invoke(new Action(() => button1.PerformClick()));
            }
        }

        private void peopleView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void ancestorsView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (ancestorsView.SelectedNode == null) return;
            if (ancestorsView.SelectedNode.Nodes.Count > 0) return;
            var p = await Humans.GetByWikiDataIDAsync((string)ancestorsView.SelectedNode.Tag);
            if (p?.Father != null)
            {
                var fNode = ancestorsView.SelectedNode.Nodes.Add("f. " + p?.Father);
                fNode.Tag = p?.Father;
                var f = await Humans.GetByWikiDataIDAsync(p?.Father);
                if (f != null && !f.Labels.IsNullOrEmpty())
                {
                    fNode.Text = string.Format("f. {0} {1}",
                        f.Labels?.Find(l => l.Language.StartsWith("en"))?.Value
                        ?? f.Labels?.First()?.Value,
                        p?.Father);
                }
                else
                {
                    fNode.Text = "f. " + "(" + p?.Father + ")";
                }

            }
            if (p?.Mother != null)
            {
                var mNode = ancestorsView.SelectedNode.Nodes.Add("m. " + p?.Mother);
                mNode.Tag = p?.Mother;
                var m = await Humans.GetByWikiDataIDAsync(p?.Mother);
                if (m != null && !m.Labels.IsNullOrEmpty())
                {
                    mNode.Text = string.Format("m. {0} {1}",
                        m.Labels?.Find(l => l.Language.StartsWith("en"))?.Value
                        ?? m.Labels?.First()?.Value,
                        p?.Mother);
                }
                else
                {
                    mNode.Text = "m. " + "(" + p?.Mother + ")";
                }
            }
            ancestorsView.SelectedNode.ExpandAll();
        }

        private void altNamesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string altName = (string)altNamesBox.SelectedItem;
            if (!altName.IsNullOrEmpty())
            {
                var parts = altName.Split(':');
                if (parts.Length > 1)
                {
                    //richTextBox1.Text = p?.Descriptions?.Where(d => d.Language.StartsWith(parts[0]))?.FirstOrDefault()?.Value;
                }
            }
        }
    }
}
