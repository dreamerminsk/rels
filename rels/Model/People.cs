using LinqToDB;
using System.Linq;

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

        public static int Insert(string wdid)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    return db.Insert(new Person() { ID = WikiDataID.ToInt(wdid), WikiDataID = wdid, Name = "???" });
                }
                catch
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

    }
}
