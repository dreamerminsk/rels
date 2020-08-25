using LinqToDB;
using LinqToDB.Common;
using rels.Model.Rest;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rels.Model.Local
{
    public class Humans
    {

        public static async Task<bool> IsExistsAsync(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var people = db.GetTable<Human>();
                return await people.AnyAsync(p => p.WikiDataID.Equals(wikiDataId));
            }
        }

        public static async Task<Human> GetByWikiDataIDAsync(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var people = db.GetTable<Human>();
                var person = await people.Where(p => p.WikiDataID.Equals(wikiDataId))?.FirstOrDefaultAsync();
                if (person != null)
                {
                    person.Labels = Labels.GetLabels(person.WikiDataID);
                    person.Descriptions = Descriptions.GetDescriptions(person.WikiDataID);
                }
                return person;
            }
        }

        public static async Task<int> InsertAsync(string wdid)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    return await db.InsertAsync(new Human() { ID = WikiDataID.ToInt(wdid), WikiDataID = wdid });
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        public static async Task<int> InsertAsync(Human p)
        {
            using (var db = new RelsDB())
            {
                return await db.InsertAsync(p); ;
            }
        }

        public static async Task<int> UpdateAsync(Human p)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    var ps = db.GetTable<Human>();
                    var res = await ps.Where(item => item.WikiDataID.Equals(p.WikiDataID))
                         .Set(item => item.ImageFile, p.ImageFile)
                         .Set(item => item.Country, p.Country)
                         .Set(item => item.DateOfBirth, p.DateOfBirth)
                         .Set(item => item.DateOfDeath, p.DateOfDeath)
                         .Set(item => item.Father, p.Father)
                         .Set(item => item.Mother, p.Mother)
                         .Set(item => item.Modified, p.Modified)
                         .UpdateAsync();
                    if (!p.Labels.IsNullOrEmpty())
                    {
                        p.Labels.ForEach(async l => await Labels.InsertAsync(l));
                    }
                    if (!p.Descriptions.IsNullOrEmpty())
                    {
                        var count = Descriptions.Insert(p.Descriptions);
                    }
                    if (!p.Siblings.IsNullOrEmpty())
                    {
                        p.Siblings.ForEach(async s => await InsertAsync(s));
                    }
                    if (!p.Spouse.IsNullOrEmpty())
                    {
                        p.Spouse.ForEach(async s => await InsertAsync(s));
                    }
                    if (!p.Children.IsNullOrEmpty())
                    {
                        p.Children.ForEach(async s => await InsertAsync(s));
                    }
                    return res;
                }
                catch (Exception e)
                {
                    MessageBox.Show("UPDATE /" + p.ID + "/\r\n" + e.Message, e.GetType().Name);
                    return -1;
                }
            }

        }
    }
}
