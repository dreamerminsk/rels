using System;

namespace rels.Model
{
    public class WikiDataID
    {

        public static string ToString(int wikiDataID)
        {
            return string.Format("Q{0}", wikiDataID);
        }

        public static int ToInt(string wikiDataID)
        {
            try
            {
                return int.Parse(wikiDataID?.Substring(1));
            }
            catch (Exception e)
            {
                return -1;
            }
        }

    }
}
