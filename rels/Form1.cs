using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using rels.Model;
using System;
using System.Collections.Generic;
using System.Data;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //listBox1.DataSource = q2;
            Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe(x => ProcessPerson());
            using (var db = new RelsDB())
            {
                Init.CREATE_SQL.ForEach(async sql => await db.ExecuteAsync(sql));

                People.Insert("Q8423");
                People.Insert("Q37085");
                People.Insert("Q202266");
                People.Insert("Q357200");
                People.Insert("Q379718");
                People.Insert("Q497773");
                People.Insert("Q7990");
                People.Insert("Q54049");
                People.Insert("Q54051");
                People.Insert("Q53435");
                People.Insert("Q9961");
                People.Insert("Q8462");
                People.Insert("Q208463");
                People.Insert("Q720");
                People.Insert("Q12591");
                People.Insert("Q156328");
                People.Insert("Q627980");
                People.Insert("Q294945");
                People.Insert("Q260783");
                People.Insert("Q299428");
                People.Insert("Q271527");
                People.Insert("Q269265");
                People.Insert("Q212671");
                People.Insert("Q335658");
                People.Insert("Q297086");
                People.Insert("Q12900494");
                People.Insert("Q371319");
                People.Insert("Q37076");
                People.Insert("Q37088");
                People.Insert("Q157099");
                People.Insert("Q7996");
                People.Insert("Q94941");
                People.Insert("Q298263");
                People.Insert("Q680304");
                People.Insert("Q743509");
                People.Insert("Q154045");
                People.Insert("Q57529");
                People.Insert("Q51068");
                People.Insert("Q6482148");
                People.Insert("Q4381410");
                People.Insert("Q6079141");
                People.Insert("Q185152");
                People.Insert("Q165096");
                People.Insert("Q212897");
                People.Insert("Q37142");
                People.Insert("Q557896");
                People.Insert("Q560157");
                People.Insert("Q49765");
                People.Insert("Q53448");
                People.Insert("Q110892");
                People.Insert("Q174964");
                People.Insert("Q214559");

                var people = db.GetTable<Person>();
                people.Where(p => (p.Name == "???"))
                    .OrderBy(x => Guid.NewGuid())
                    .ToList().ForEach(p => q2.Add(p.WikiDataID));


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
                AppendText(string.Format("{0}\r\n{1}\r\n{2}\r\n", new string('-', 64), p.WikiDataID, p.Name));
                AppendText(string.Format("  {0}\r\n", p.RusName));
                AppendText(string.Format("{0}\r\n", p.Description));
                if (Countries.IsExists(p.Country))
                {
                    AppendText(string.Format("\tCountry:\t{0} - {1}\r\n", p.Country, Countries.GetByWikiDataId(p.Country)?.Name));
                }
                else if (!p.Country.IsNullOrEmpty())
                {
                    var c = await WikiData.GetCountryAsync(p.Country);
                    await Countries.InsertAsync(c);
                    AppendText(string.Format("\tCountry:\t{0} - {1}\r\n", p.Country, Countries.GetByWikiDataId(p.Country)?.Name));
                }
                AppendText(string.Format("\tDate Of Birth:\t{0}\r\n", p.DateOfBirth));
                AppendText(string.Format("\tDate Of Death:\t{0}\r\n", p.DateOfDeath));
                AppendText(string.Format("\tFather:\t{0}\r\n", p.Father));
                AppendText(string.Format("\tMother:\t{0}\r\n", p.Mother));
                p.Siblings.ForEach(s => AppendText(string.Format("\tSibling:\t{0}\r\n", s)));
                p.Spouse.ForEach(s => AppendText(string.Format("\tSpouse:\t{0}\r\n", s)));
                p.Children.ForEach(s => AppendText(string.Format("\tChild:\t{0}\r\n", s)));
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

        private void AppendText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() =>
                {
                    richTextBox1.AppendText(text);
                }));
            }
            else
            {
                richTextBox1.AppendText(text);
            }
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
                Clear();
                var ps = db.GetTable<Person>();
                var bs = ps.GroupBy(p => p.DateOfBirth.Substring(0, 3));
                bs.ForEachAsync(g => AddItem(g.Key, g.Count()));
            }
        }

        private void Clear()
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => { listView1.Items.Clear(); }));
            }
            else
            {
                listView1.Items.Clear();
            }
        }

        private void AddItem(string name, int value)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => { var li = listView1.Items.Add(name); li.SubItems.Add(value.ToString()); }));
            }
            else
            {
                var li = listView1.Items.Add(name); li.SubItems.Add(value.ToString());
            }
        }
    }
}
