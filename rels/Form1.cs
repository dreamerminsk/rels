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

                var people = db.GetTable<Person>();
                people.Where(p => (p.Father == null) && (p.Mother == null))
                    .OrderBy(x => Guid.NewGuid())
                    .ToList().ForEach(p => q2.Add(p.WikiDataID));

                q2.Add("Q7996");
                q2.Add("Q298263");
                q2.Add("Q680304");
                q2.Add("Q743509");
                q2.Add("Q154045");
                q2.Add("Q57529");
                q2.Add("Q51068");
                q2.Add("Q6482148");
                q2.Add("Q4381410");
                q2.Add("Q6079141");
                q2.Add("Q185152");
                q2.Add("Q165096");
                q2.Add("Q212897");
                q2.Add("Q37142");
            }
        }

        private async void ProcessPerson()
        {
            if (q2.IsNullOrEmpty()) return;
            var title = q2[0];
            q2.RemoveAt(0);
            UpdateQueue();
            if (!string.IsNullOrEmpty(title))
            {
                var p = await WikiData.GetPersonAsync(title);
                using (var db = new RelsDB())
                {
                    try
                    {
                        int res = await db.InsertAsync(p);
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.Message, e.GetType().Name);
                    }
                }
                AppendText(string.Format("{0}\r\n{1}\r\n", new string('-', 64), p.Name));
                AppendText(string.Format("  {0}\r\n", p.RusName));
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
                if (!People.IsExists(p.Father))
                {
                    q2.Add(p.Father);
                    //UpdateQueue();
                }
                if (!People.IsExists(p.Mother))
                {
                    q2.Add(p.Mother);
                    //UpdateQueue();
                }
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

        private void UpdateQueue()
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() =>
                {
                    UpdateListBox();
                }));
            }
            else
            {
                UpdateListBox();
            }
        }

        private void UpdateListBox()
        {
            var si = listBox1.SelectedIndex;
            listBox1.BeginUpdate();
            listBox1.DataSource = q2;
            listBox1.SelectedIndex = si;
            listBox1.EndUpdate();
            listBox1.Update();
            SetTitle("QUEUE / " + q2.Count + " /");
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
    }
}
