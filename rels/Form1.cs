using HtmlAgilityPack;
using LinqToDB;
using LinqToDB.Common;
using rels.Model;
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
            //UpdateQueue();
            using (var db = new RelsDB())
            {
                var sp = db.DataProvider.GetSchemaProvider();
                var dbSchema = sp.GetSchema(db);
                if (!dbSchema.Tables.Any(t => t.TableName == "People"))
                {
                    db.CreateTable<Person>();
                }
                else
                {
                    var people = db.GetTable<Person>();
                    people.Where(p => (p.Father == null) && (p.Mother == null))
                        .OrderBy(x => Guid.NewGuid())
                        //.Take(16)
                        .ToList().ForEach(p => q.Enqueue(p.WikiDataID));
                }
                q.Enqueue("Q298263");
                q.Enqueue("Q680304");
                //q.Enqueue("Q743509");
                //q.Enqueue("Q154045");
                //q.Enqueue("Q57529");
                //q.Enqueue("Q51068");
                //q.Enqueue("Q6482148");
                //q.Enqueue("Q4381410");
                //q.Enqueue("Q6079141");
                //q.Enqueue("Q185152");
            }
        }

        private async void ProcessPerson()
        {
            if (q.IsNullOrEmpty()) return;
            var title = q.Dequeue();
            if (!string.IsNullOrEmpty(title))
            {
                var p = await WikiData.GetPersonAsync(title);
                using (var db = new RelsDB())
                {
                    int res = await db.InsertAsync(p);
                }
                AppendText(string.Format("{0}\r\n", p.Name));
                AppendText(string.Format("  {0}\r\n", p.RusName));
                AppendText(string.Format("\tCountry:\t{0}\r\n", p.Country));
                AppendText(string.Format("\tDate Of Birth:\t{0}\r\n", p.DateOfBirth));
                AppendText(string.Format("\tDate Of Death:\t{0}\r\n", p.DateOfDeath));
                AppendText(string.Format("\tFather:\t{0}\r\n", p.Father));
                AppendText(string.Format("\tMother:\t{0}\r\n", p.Mother));
                if (!processed.Contains(p.Father))
                {
                    q.Enqueue(p.Father);
                }
                if (!processed.Contains(p.Mother))
                {
                    q.Enqueue(p.Mother);
                }
                UpdateQueue();
            }
        }

        private void AppendText(string text)
        {
            richTextBox1.Invoke(new Action(() => { richTextBox1.AppendText(text); }));
        }

        private void UpdateQueue()
        {
            listBox1.Invoke(new Action(() =>
            {
                var si = listBox1.SelectedIndex;
                listBox1.BeginUpdate();
                listBox1.DataSource = q.ToList();
                listBox1.SelectedIndex = si;
                listBox1.EndUpdate();
                SetTitle("QUEUE / " + q.Count + " /");
            }));

        }

        private void SetTitle(string text)
        {
            this.Invoke(new Action(() => { this.Text = text; }));
        }
    }
}
