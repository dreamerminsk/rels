using LinqToDB.Common;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace rels.Model
{
    [Table(Name = "People")]
    public class Person
    {
        private string _name = "en: ???????";

        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true)]
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
