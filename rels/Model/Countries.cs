using LinqToDB;
using System.Linq;
using System.Threading.Tasks;

namespace rels.Model
{
    public class Countries
    {

        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024
        });

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

        public static async Task<int> InsertAsync(Country c)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    return await db.InsertAsync(c);
                }
                catch
                {
                    return -1;
                }
            }
        }

    }
}
