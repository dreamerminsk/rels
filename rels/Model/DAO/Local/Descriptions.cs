using LinqToDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace rels.Model.Local
{
    public class Descriptions
    {

        public static int Insert(List<Description> ds)
        {
            using (var db = new RelsDB())
            {
                return ds.Select(async d =>
                {
                    try
                    {
                        return await db.InsertAsync(d);
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.Message, e.GetType().Name);
                        return 0;
                    }
                }).Sum(t => t.Result);

            }
        }

        public static async Task<int> InsertAsync(Description l)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    return await db.InsertAsync(l);
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        public static List<Description> GetDescriptions(string wikiDataID)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    var descriptions = db.GetTable<Description>();
                    return descriptions.Where(l => l.WikiDataID.Equals(wikiDataID)).ToList();
                }
                catch (Exception e)
                {
                    return new List<Description>();
                }
            }
        }
    }
}