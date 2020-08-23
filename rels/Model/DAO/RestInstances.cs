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
    public class RestInstances
    {

        private const string INSTANCES_ENDPOINT = "http://172.105.80.145:8000/api/rels/instances";

        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024 * 1024
        });

        public static async Task<bool> IsExistsAsync(string wikiDataId)
        {
            if (wikiDataId == null) return false;
            Instance cacheEntry;
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

        public static async Task<Instance> GetByWikiDataIdAsync(string wikiDataId)
        {
            Instance cacheEntry;
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

        private static async Task<Instance> QueryByWikiDataIdAsync(string wikiDataId)
        {
            try
            {
                var page = await Web.GetStringAsync(string.Format("{0}/{1}", INSTANCES_ENDPOINT, wikiDataId));
                var doc = JObject.Parse(page);
                return new Instance
                {
                    WikiDataID = doc["WikiDataID"]?.ToString(),
                    Name = doc["Name"]?.ToString(),
                    RusName = doc["RusName"]?.ToString(),
                };
            }
            catch
            {
                return new Instance
                {
                    WikiDataID = wikiDataId,
                };
            }

        }

        public static async Task<int> InsertAsync(Instance c)
        {
            var page = await Web.PostStringAsync(INSTANCES_ENDPOINT, JsonConvert.SerializeObject(c));
            return -1;
        }

    }
}
