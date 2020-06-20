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

    }
}
