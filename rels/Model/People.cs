using LinqToDB;
using LinqToDB.Common;
using System;
using System.Linq;
using System.Windows.Forms;

namespace rels.Model
{
    public class People
    {

        public static bool IsExists(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var people = db.GetTable<Person>();
                return people.Any(p => p.WikiDataID.Equals(wikiDataId));
            }
        }

        public static Person GetByWikiDataID(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var people = db.GetTable<Person>();
                var person = people.Where(p => p.WikiDataID.Equals(wikiDataId)).First();
                if (person != null)
                {
                    person.Labels = Labels.GetLabels(person.WikiDataID);
                }
                return person;
            }
        }

        public static int Insert(string wdid)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    return db.Insert(new Person() { ID = WikiDataID.ToInt(wdid), WikiDataID = wdid });
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        public static int Insert(Person p)
        {
            using (var db = new RelsDB())
            {
                return db.Insert(p); ;
            }
        }

        public static int Update(Person p)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    var ps = db.GetTable<Person>();
                    var res = ps.Where(item => item.WikiDataID.Equals(p.WikiDataID))
                         .Set(item => item.ImageFile, p.ImageFile)
                         .Set(item => item.Country, p.Country)
                         .Set(item => item.DateOfBirth, p.DateOfBirth)
                         .Set(item => item.DateOfDeath, p.DateOfDeath)
                         .Set(item => item.Father, p.Father)
                         .Set(item => item.Mother, p.Mother)
                         .Set(item => item.Description, p.Description)
                         .Update();
                    if (!p.Labels.IsNullOrEmpty())
                    {
                        p.Labels.ForEach(l => Labels.Insert(l));
                    }
                    if (!p.Siblings.IsNullOrEmpty())
                    {
                        p.Siblings.ForEach(s => People.Insert(s));
                    }
                    if (!p.Spouse.IsNullOrEmpty())
                    {
                        p.Spouse.ForEach(s => People.Insert(s));
                    }
                    if (!p.Children.IsNullOrEmpty())
                    {
                        p.Children.ForEach(s => People.Insert(s));
                    }
                    return res;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, e.GetType().Name);
                    return -1;
                }
            }
        }
    }
}
