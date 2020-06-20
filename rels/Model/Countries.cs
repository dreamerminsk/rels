using LinqToDB;
using System.Linq;

namespace rels.Model
{
    public class Countries
    {

        public static bool IsExists(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var countries = db.GetTable<Country>();
                return countries.Any(p => p.WikiDataID.Equals(wikiDataId));
            }
        }

        public static Country GetByWikiDataId(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var countries = db.GetTable<Country>();
                return countries.Where(p => p.WikiDataID.Equals(wikiDataId)).FirstOrDefault();
            }
        }

        public static int Insert(Country c)
        {
            using (var db = new RelsDB())
            {
                return db.Insert(c);
            }
        }

    }
}
