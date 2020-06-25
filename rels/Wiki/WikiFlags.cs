using System.Drawing;
using System.Threading.Tasks;

namespace rels.Wiki
{
    public class WikiFlags
    {

        private static readonly string ENG = "https://upload.wikimedia.org/wikipedia/en/thumb/a/ae/Flag_of_the_United_Kingdom.svg/20px-Flag_of_the_United_Kingdom.svg.png";

        private static readonly string RUS = "https://upload.wikimedia.org/wikipedia/en/thumb/f/f3/Flag_of_Russia.svg/16px-Flag_of_Russia.svg.png";

        public static async Task<Image> GetEnglishAsync()
        {
            return await WikiMedia.GetImageAsync(ENG).ConfigureAwait(false);
        }

        public static async Task<Image> GetRussianAsync()
        {
            return await WikiMedia.GetImageAsync(RUS).ConfigureAwait(false);
        }
    }
}
