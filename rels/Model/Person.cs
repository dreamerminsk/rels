using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rels.Model
{
    [Table(Name = "People")]
    public class Person
    {
        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        [Column(Name="WikiDataID")]
        public string WikiDataID { get; set; }

        [Column(Name = "Name"), NotNull]
        public string Name { get; set; }

        [Column(Name = "RusName")]
        public string RusName { get; set; }

        [Column(Name="DateOfBirth")]
        public string DateOfBirth { get; set; }

        [Column(Name = "DateOfDeath")]
        public string DateOfDeath { get; set; }

        [Column(Name = "Father")]
        public string Father { get; set; }

        [Column(Name = "Mother")]
        public string Mother { get; set; }
    }
}
