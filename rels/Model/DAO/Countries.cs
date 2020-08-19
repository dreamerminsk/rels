using LinqToDB;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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

        public static async Task<bool> IsExistsAsync(string wikiDataId)
        {
            if (wikiDataId == null) return true;
            Country cacheEntry;
            if (!_cache.TryGetValue(wikiDataId, out cacheEntry))
            {
                cacheEntry = await QueryByWikiDataIdAsync(wikiDataId);
                if (cacheEntry.Name.StartsWith("en: "))
                {
                    return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public static async Task<Country> GetByWikiDataIdAsync(string wikiDataId)
        {
            Country cacheEntry;
            if (!_cache.TryGetValue(wikiDataId, out cacheEntry))
            {
                cacheEntry = await QueryByWikiDataIdAsync(wikiDataId);

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

        private static async Task<Country> QueryByWikiDataIdAsync(string wikiDataId)
        {
            try
            {
                var page = await Web.GetStringAsync(string.Format("{0}{1}", COUNTRIES_ENDPOINT, wikiDataId));
                var doc = JObject.Parse(page);
                return new Country
                {
                    WikiDataID = doc["WikiDataID"]?.ToString(),
                    Name = doc["Name"]?.ToString(),
                    RusName = doc["RusName"]?.ToString(),
                };
            }
            catch
            {
                return new Country
                {
                    WikiDataID = wikiDataId,
                };
            }

        }

        public static async Task<int> InsertAsync(Country c)
        {
            var page = await Web.PostStringAsync(COUNTRIES_ENDPOINT, JsonConvert.SerializeObject(c));
            return -1;
        }

    }
}
