
using LinqToDB.Common;
using rels.Model;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace rels.Workers
{

    public class Updater
    {

        private int isRunning = 0;

        private DateTime started = DateTime.Now;

        private Subject<string> wikiData = new Subject<string>();

        public IObservable<string> WikiData =>
            wikiData.AsObservable();

        public Updater()
        {
        }

        public ConcurrentQueue<string> q { get; } = new ConcurrentQueue<string>();

        public void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x => ProcessPerson());
        }

        private async void ProcessPerson()
        {
            try
            {
                if (Interlocked.CompareExchange(ref isRunning, 1, 0) == 1) { return; }
                var now = DateTime.Now;
                if (now.Subtract(started).TotalSeconds < 8)
                {
                    return;
                }
                started = now;
                string title = null;
                if (!q.TryDequeue(out title)) { ReloadQueue(); return; }
                if (string.IsNullOrEmpty(title)) { return; }
                wikiData.OnNext(title);
                var p = await Wiki.WikiData.GetPersonAsync(title);
                await Humans.UpdateAsync(p);
                if (Countries.IsExists(p.Country))
                {

                }
                else if (!p.Country.IsNullOrEmpty())
                {
                    var c = await Wiki.WikiData.GetCountryAsync(p.Country);
                    await Countries.InsertAsync(c);
                }
                if (!await Humans.IsExistsAsync(p.Father))
                {
                    if (!p.Father.IsNullOrEmpty())
                    {
                        await Humans.InsertAsync(p.Father);
                    }
                }
                if (!await Humans.IsExistsAsync(p.Mother))
                {
                    if (!p.Mother.IsNullOrEmpty())
                    {
                        await Humans.InsertAsync(p.Mother);
                    }
                }
            }
            catch (Exception e)
            {
                wikiData.OnError(e);
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
                var people = db.GetTable<Human>();
                people.Where(p => p.Labels.Count == 0)
                    .OrderBy(x => Guid.NewGuid())
                    .ToList().ForEach(p => q.Enqueue(p.WikiDataID));
            }
        }
    }

}
