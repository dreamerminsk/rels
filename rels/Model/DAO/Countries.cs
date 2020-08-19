using LinqToDB;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using rels.Wiki;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace rels.Model
{
    public class Countries
    {
        private const string COUNTRIES_ENDPOINT = "http://172.105.80.145:8000/api/rels/countries/";
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024 * 1024
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
            Country cacheEntry;
            if (!_cache.TryGetValue(wikiDataId, out cacheEntry))
            {
                cacheEntry = QueryByWikiDataId(wikiDataId);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(cacheEntry.Name.Length + cacheEntry.RusName.Length)
                    .SetPriority(CacheItemPriority.High)
                    .SetSlidingExpiration(TimeSpan.FromSeconds(600));

                _cache.Set(wikiDataId, cacheEntry, cacheEntryOptions);
            }
            return cacheEntry;
        }

        private static Country QueryByWikiDataId(string wikiDataId)
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

        private static async Task<Country> QueryByWikiDataIdAsync(string wikiDataId)
        {
            var page = await Web.GetStringAsync(string.Format("{0}{1}", COUNTRIES_ENDPOINT, wikiDataId));
            var doc = JObject.Parse(page);
            return new Country
            {
                WikiDataID = doc["WikiDataID"].ToString(),
                Name = doc["Name"].ToString(),
                RusName = doc["RusName"].ToString(),
            };
        }

        public static async Task<int> InsertAsync2(Country c)
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
