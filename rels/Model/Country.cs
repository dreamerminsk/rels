using LinqToDB.Common;
using LinqToDB.Mapping;

namespace rels.Model
{
    [Table(Name = "Countries")]
    public class Country
    {
        private string _name = "en: ???????";

        [PrimaryKey, Identity]
        public int ID { get; set; }

        [Column(Name = "WikiDataID")]
        public string WikiDataID { get; set; }

        [Column(Name = "Name"), NotNull]
        public string Name { get { return _name; } set { if (!value.IsNullOrEmpty()) _name = value; } }

        [Column(Name = "RusName")]
        public string RusName { get; set; } = "ru: ???????";

    }
}
