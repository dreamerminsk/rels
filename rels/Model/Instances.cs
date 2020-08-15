using LinqToDB;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace rels.Model
{
    public class Instances
    {

        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024 * 1024
        });

        public static bool IsExists(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var instances = db.GetTable<Instance>();
                return instances.Any(p => p.WikiDataID.Equals(wikiDataId));
            }
        }

        public static Instance GetByWikiDataId(string wikiDataId)
        {
            using (var db = new RelsDB())
            {
                var instances = db.GetTable<Instance>();
                return instances.Where(p => p.WikiDataID.Equals(wikiDataId)).FirstOrDefault();
            }
        }

        public static async Task<int> InsertAsync(Instance c)
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
