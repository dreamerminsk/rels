
using LinqToDB.Common;
using rels.Model;
using rels.Wiki;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;

namespace rels.Workers
{

    public class Updater
    {

        private int isRunning = 0;

        private DateTime started = DateTime.Now;

        public Updater()
        {
        }

        public ConcurrentQueue<string> q { get; } = new ConcurrentQueue<string>();

        public void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(10)).Subscribe(x => ProcessPerson());
        }

        private async void ProcessPerson()
        {
            try
            {
                if (Interlocked.CompareExchange(ref isRunning, 1, 0) == 1) { return; }
                var now = DateTime.Now;
                MessageBox.Show(now.Subtract(started).TotalSeconds.ToString());
                started = now;
                string title = null;
                if (!q.TryDequeue(out title)) { ReloadQueue(); return; }
                if (string.IsNullOrEmpty(title)) { return; }
                var p = await WikiData.GetPersonAsync(title);
                MessageBox.Show(p.WikiDataID);
                await People.UpdateAsync(p);
                if (Countries.IsExists(p.Country))
                {

                }
                else if (!p.Country.IsNullOrEmpty())
                {
                    var c = await WikiData.GetCountryAsync(p.Country);
                    await Countries.InsertAsync(c);
                }
                if (!await People.IsExistsAsync(p.Father))
                {
                    if (!p.Father.IsNullOrEmpty())
                    {
                        await People.InsertAsync(p.Father);
                    }
                }
                if (!await People.IsExistsAsync(p.Mother))
                {
                    if (!p.Mother.IsNullOrEmpty())
                    {
                        await People.InsertAsync(p.Mother);
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref isRunning, 0);
            }
        }

        private void ReloadQueue()
        {
            using (var db = new RelsDB())
            {
                var people = db.GetTable<Person>();
                people.Where(p => p.Labels.Count == 0)
                    .OrderBy(x => Guid.NewGuid())
                    .ToList().ForEach(p => q.Enqueue(p.WikiDataID));
            }
        }
    }

}
