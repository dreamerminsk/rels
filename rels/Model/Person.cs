using LinqToDB.Common;
using LinqToDB.Mapping;

namespace rels.Model
{
    [Table(Name = "People")]
    public class Person
    {
        private string _name = "en: ???????";

        [PrimaryKey, Identity]
        public int ID { get; set; }

        [Column(Name = "WikiDataID")]
        public string WikiDataID { get; set; }

        [Column(Name = "Name"), NotNull]
        public string Name { get { return _name; } set { if (!value.IsNullOrEmpty()) _name = value; } }

        [Column(Name = "RusName")]
        public string RusName { get; set; } = "ru:???????";

        [Column(Name = "Country")]
        public string Country { get; set; } = "???????";

        [Column(Name = "DateOfBirth")]
        public string DateOfBirth { get; set; }

        [Column(Name = "DateOfDeath")]
        public string DateOfDeath { get; set; }

        [Column(Name = "Father")]
        public string Father { get; set; }

        [Column(Name = "Mother")]
        public string Mother { get; set; }
    }
}
