using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using rels.Model;
using rels.UI;
using rels.Wiki;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace rels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //public Queue<string> q = new Queue<string>();

        public List<string> q2 = new List<string>();

        private async void Form1_Load(object sender, EventArgs e)
        {
            Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe(x => ProcessPerson());
            Observable.Interval(TimeSpan.FromSeconds(32)).Subscribe(x => UpdateStats());
            button1.PerformClick();
            nameFlag.Image = await WikiFlags.GetEnglishAsync(18).ConfigureAwait(true);
            nameFlag.Width = nameFlag.Image.Width + 2;
            nameFlag.Height = nameFlag.Image.Height + 2;
            rusNameFlag.Image = await WikiFlags.GetRussianAsync(18).ConfigureAwait(true);
            rusNameFlag.Width = rusNameFlag.Image.Width + 2;
            rusNameFlag.Height = rusNameFlag.Image.Height + 2;
            using (var db = new RelsDB())
            {
                Init.CREATE_SQL.ForEach(async sql => await db.ExecuteAsync(sql));

                People.Insert("Q743509");//Aiko
                //People.Insert("Q154045");//Alexei
                //People.Insert("Q8423");
                //People.Insert("Q37085");
                //People.Insert("Q202266");
                //People.Insert("Q317997");
                //People.Insert("Q316221");
                //People.Insert("Q357200");
                //People.Insert("Q379718");
                //People.Insert("Q497773");
                //People.Insert("Q7990");
                //People.Insert("Q44228");
                //People.Insert("Q54049");
                //People.Insert("Q54051");
                //People.Insert("Q53435");
                //People.Insert("Q9961");
                //People.Insert("Q8462");
                //People.Insert("Q208463");
                //People.Insert("Q1660081");
                //People.Insert("Q720");
                //People.Insert("Q12591");
                //People.Insert("Q156328");
                //People.Insert("Q627980");
                //People.Insert("Q294945");
                //People.Insert("Q233224");
                //People.Insert("Q260783");
                //People.Insert("Q299428");
                //People.Insert("Q271527");
                //People.Insert("Q269265");
                //People.Insert("Q212671");
                //People.Insert("Q171977");
                //People.Insert("Q243122");
                //People.Insert("Q335658");
                //People.Insert("Q309946");
                //People.Insert("Q297086");
                //People.Insert("Q12900494");
                //People.Insert("Q200188");
                //People.Insert("Q371319");
                //People.Insert("Q37076");
                //People.Insert("Q37088");
                //People.Insert("Q157099");
                //People.Insert("Q144565");
                //People.Insert("Q7996");
                //People.Insert("Q94941");
                //People.Insert("Q335273");
                //People.Insert("Q21932460");
                //People.Insert("Q298263");
                //People.Insert("Q680304");
                //People.Insert("Q312938");
                //People.Insert("Q151826");
                //People.Insert("Q743509");
                //People.Insert("Q313298");
                //People.Insert("Q154045");
                //People.Insert("Q57529");
                //People.Insert("Q51068");
                //People.Insert("Q2658842");
                //People.Insert("Q6482148");
                //People.Insert("Q105105");
                //People.Insert("Q293626");
                //People.Insert("Q2635189");
                //People.Insert("Q4459448");
                //People.Insert("Q4381410");
                //People.Insert("Q315191");
                //People.Insert("Q6079141");
                //People.Insert("Q185152");
                //People.Insert("Q1284160");
                //People.Insert("Q1385871");
                //People.Insert("Q165096");
                //People.Insert("Q284750");
                //People.Insert("Q212897");
                //People.Insert("Q349440");
                //People.Insert("Q37142");
                //People.Insert("Q557896");
                //People.Insert("Q560157");
                //People.Insert("Q49765");
                //People.Insert("Q379239");
                //People.Insert("Q53448");
                //People.Insert("Q110892");
                //People.Insert("Q471885");
                //People.Insert("Q174964");
                //People.Insert("Q214559");
                //People.Insert("Q320229");
                //People.Insert("Q333603");

                Random rnd = new Random(DateTime.Now.Millisecond);
                var people = db.GetTable<Person>();
                var listOf = people.Where(p => (p.Name == "???")).OrderBy(p => Guid.NewGuid()).ToList();
                listOf.ForEach(p => q2.Add(p.WikiDataID));
            }
        }

        private async void ProcessPerson()
        {
            if (q2.IsNullOrEmpty()) { ReloadQueue(); return; }
            var title = q2[0];
            q2.RemoveAt(0);
            SetTitle(string.Format("QUEUE /{0}/", q2.Count));
            if (!string.IsNullOrEmpty(title))
            {
                var p = await WikiData.GetPersonAsync(title);
                People.Update(p);
                string desc = "";
                desc = string.Format("{0}\r\n{1}\r\n{2}\r\n", new string('-', 64), p.WikiDataID, p.Name);
                desc += string.Format("  {0}\r\n", p.RusName);
                desc += string.Format("{0}\r\n", p.Description);
                if (Countries.IsExists(p.Country))
                {
                    desc += (string.Format("\tCountry:\t{0} - {1}\r\n", p.Country, Countries.GetByWikiDataId(p.Country)?.Name));
                }
                else if (!p.Country.IsNullOrEmpty())
                {
                    var c = await WikiData.GetCountryAsync(p.Country);
                    await Countries.InsertAsync(c);
                    desc += (string.Format("\tCountry:\t{0} - {1}\r\n", p.Country, Countries.GetByWikiDataId(p.Country)?.Name));
                }
                desc += (string.Format("\tDate Of Birth:\t{0}\r\n", p.DateOfBirth));
                desc += (string.Format("\tDate Of Death:\t{0}\r\n", p.DateOfDeath));
                desc += (string.Format("\tFather:\t{0}\r\n", p.Father));
                desc += (string.Format("\tMother:\t{0}\r\n", p.Mother));
                AppendPerson(p);
                if (!People.IsExists(p.Father))
                {
                    if (!p.Father.IsNullOrEmpty())
                    {
                        People.Insert(p.Father);
                    }
                }
                if (!People.IsExists(p.Mother))
                {
                    if (!p.Mother.IsNullOrEmpty())
                    {
                        People.Insert(p.Mother);
                    }
                }
            }
        }

        private void ReloadQueue()
        {
            using (var db = new RelsDB())
            {
                var people = db.GetTable<Person>();
                people.Where(p => (p.Name == "???"))
                    .OrderBy(x => Guid.NewGuid())
                    .ToList().ForEach(p => q2.Add(p.WikiDataID));
            }
        }

        private void AppendPerson(Person p)
        {
            if (peopleView.InvokeRequired)
            {
                peopleView.Invoke(new Action(() =>
                {
                    SetPerson(p);
                }));
            }
            else
            {
                SetPerson(p);
            }
        }

        private void SetPerson(Person p)
        {
            peopleView.BeginUpdate();
            var item = peopleView.Items.Insert(0, p.WikiDataID);
            item.SubItems.Add(p.Name);
            item.SubItems.Add(p.RusName);
            item.SubItems.Add(p.DateOfBirth?.Substring(0, 11));
            item.SubItems.Add(p.DateOfDeath?.Substring(0, 11));
            peopleView.EndUpdate();
        }

        private void SetTitle(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => { this.Text = text; }));
            }
            else
            {
                this.Text = text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new RelsDB())
            {
                var ps = db.GetTable<Person>();

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
            using (var db = new RelsDB())
            {
                var ps = db.GetTable<Person>();
                var bs = ps.GroupBy(p => p.Country);
                var cs = new Dictionary<string, int>();
                bs.ForEachAsync(g => cs.Add(Countries.GetByWikiDataId(g.Key)?.Name ?? "UNKNOWN", g.Count()));
                UpdateCountries(cs);
            }
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

        private async void peopleView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (peopleView.SelectedItems.Count > 0)
            {
                var wikiDataID = peopleView.SelectedItems[0].Text;
                var p = People.GetByWikiDataID(wikiDataID);
                nameLabel.Text = p.Name;
                nameFlag.Left = nameLabel.Left + nameLabel.Width + 4;
                rusNameLabel.Text = p.RusName;
                rusNameFlag.Left = rusNameLabel.Left + rusNameLabel.Width + 4;
                pictureBox1.Image = await WikiMedia.GetMediaAsync(p.ImageFile).ConfigureAwait(true);
                richTextBox1.Text = p.Description;
            }
        }
    }
}
