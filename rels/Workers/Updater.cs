
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
        private static readonly DateTime DEFAULT = DateTime.Parse("0001-01-01 00:00:00");

        private int isRunning = 0;

        private DateTime started = DateTime.Now;

        private Subject<string> wikiData = new Subject<string>();

        public IObservable<string> WikiData =>
            wikiData.AsObservable();


        private Subject<string> log = new Subject<string>();

        public IObservable<string> Log =>
            log.AsObservable();

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
                if (now.Subtract(started).TotalSeconds < 12)
                {
                    return;
                }
                started = now;
                string title = null;
                if (!q.TryDequeue(out title)) { ReloadQueue(); return; }
                if (string.IsNullOrEmpty(title)) { return; }
                wikiData.OnNext(title);
                log.OnNext(string.Format("{0} - {1}\r\n", started.ToLongTimeString(), title));
                var p = await Wiki.WikiData.GetPersonAsync(title);


                if (await Instances.IsExistsAsync(p.Instance))
                {
                    var i = await Instances.GetByWikiDataIdAsync(p.Instance);
                    log.OnNext(string.Format("\tinstance : {0} / {1}\r\n", i.Name, i.RusName));
                }
                else if (!p.Instance.IsNullOrEmpty())
                {
                    var i = await Wiki.WikiData.GetInstanceAsync(p.Instance);
                    log.OnNext(string.Format("\tinstance : {0} / {1}\r\n", i.Name, i.RusName));
                    await Instances.InsertAsync(i);
                }

                p.Labels.ForEach(l =>
                {
                    if (l.Language.Equals("en")) log.OnNext(string.Format("\ten : {0}\r\n", l.Value));
                    if (l.Language.Equals("ru")) log.OnNext(string.Format("\tru : {0}\r\n", l.Value));
                });
                if (!p.DateOfBirth.IsNullOrEmpty()) log.OnNext(string.Format("\tbirth : {0}\r\n", p.DateOfBirth));
                if (!p.DateOfDeath.IsNullOrEmpty()) log.OnNext(string.Format("\tdeath : {0}\r\n", p.DateOfDeath));
                await Humans.UpdateAsync(p);
                if (await Countries.IsExistsAsync(p.Country))
                {
                    var c = await Countries.GetByWikiDataIdAsync(p.Country);
                    log.OnNext(string.Format("\tcountry : {0} / {1}\r\n", c.Name, c.RusName));
                }
                else if (!p.Country.IsNullOrEmpty())
                {
                    var c = await Wiki.WikiData.GetCountryAsync(p.Country);
                    log.OnNext(string.Format("\tcountry : {0} / {1}\r\n", c.Name, c.RusName));
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
                people.Where(p => p.Modified.Equals(DEFAULT))
                    .OrderBy(p => p.WikiDataID.Length)
                    .ToList().ForEach(p => q.Enqueue(p.WikiDataID));
            }
        }
    }

}
