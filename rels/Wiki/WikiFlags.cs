using System.Drawing;
using System.Threading.Tasks;

namespace rels.Wiki
{
    public class WikiFlags
    {

        private static readonly string ENG = "https://upload.wikimedia.org/wikipedia/en/thumb/b/be/Flag_of_England.svg/{0}px-Flag_of_England.svg.png";

        private static readonly string RUS = "https://upload.wikimedia.org/wikipedia/en/thumb/f/f3/Flag_of_Russia.svg/{0}px-Flag_of_Russia.svg.png";

        public static async Task<Image> GetEnglishAsync(int size)
        {
            return await WikiMedia.GetImageAsync(string.Format(ENG, size)).ConfigureAwait(false);
        }

        public static async Task<Image> GetRussianAsync(int size)
        {
            return await WikiMedia.GetImageAsync(string.Format(RUS, size)).ConfigureAwait(false);
        }
    }
}
