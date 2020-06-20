using LinqToDB;
using LinqToDB.Common;
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

        public Queue<string> q = new Queue<string>();

        private List<string> countriesQueue = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe(x => ProcessPerson());
            //UpdateQueue();
            using (var db = new RelsDB())
            {
                var sp = db.DataProvider.GetSchemaProvider();
                var dbSchema = sp.GetSchema(db);
                if (!dbSchema.Tables.Any(t => t.TableName == "Countries"))
                {
                    db.CreateTable<Country>();
                }
                if (!dbSchema.Tables.Any(t => t.TableName == "People"))
                {
                    db.CreateTable<Person>();
                }
                else
                {
                    var people = db.GetTable<Person>();
                    people.Where(p => (p.Father == null) && (p.Mother == null))
                        .OrderBy(x => Guid.NewGuid())
                        .Take(32)
                        .ToList().ForEach(p => q.Enqueue(p.WikiDataID));
                }
                q.Enqueue("Q298263");
                q.Enqueue("Q680304");
                q.Enqueue("Q743509");
                q.Enqueue("Q154045");
                q.Enqueue("Q57529");
                q.Enqueue("Q51068");
                q.Enqueue("Q6482148");
                q.Enqueue("Q4381410");
                q.Enqueue("Q6079141");
                q.Enqueue("Q185152");
            }
        }

        private async void ProcessPerson()
        {
            if (q.IsNullOrEmpty()) return;
            var title = q.Dequeue();
            UpdateQueue();
            if (!string.IsNullOrEmpty(title))
            {
                var p = await WikiData.GetPersonAsync(title);
                using (var db = new RelsDB())
                {
                    int res = await db.InsertAsync(p);
                }
                AppendText(string.Format("{0}\r\n{1}\r\n", new string('-', 64), p.Name));
                AppendText(string.Format("  {0}\r\n", p.RusName));
                if (Countries.IsExists(p.Country))
                {
                    AppendText(string.Format("\tCountry:\t{0} - {1}\r\n", p.Country, Countries.GetByWikiDataId(p.Country).Name));
                }
                else
                {
                    var c = await WikiData.GetCountryAsync(p.Country);
                    Countries.Insert(c);
                    AppendText(string.Format("\tCountry:\t{0} - {1}\r\n", p.Country, Countries.GetByWikiDataId(p.Country).Name));
                }
                AppendText(string.Format("\tDate Of Birth:\t{0}\r\n", p.DateOfBirth));
                AppendText(string.Format("\tDate Of Death:\t{0}\r\n", p.DateOfDeath));
                AppendText(string.Format("\tFather:\t{0}\r\n", p.Father));
                AppendText(string.Format("\tMother:\t{0}\r\n", p.Mother));
                if (!People.IsExists(p.Father))
                {
                    q.Enqueue(p.Father);
                    //UpdateQueue();
                }
                if (!People.IsExists(p.Mother))
                {
                    q.Enqueue(p.Mother);
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
                UpdateListBox();
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
            listBox1.DataSource = q.ToList();
            listBox1.SelectedIndex = si;
            listBox1.EndUpdate();
            SetTitle("QUEUE / " + q.Count + " /");
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
