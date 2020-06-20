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
            return int.Parse(wikiDataID.Substring(1));
        }

    }
}
