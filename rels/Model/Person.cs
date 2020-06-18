using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rels.Model
{
    [Table(Name="People")]
    public class Person
    {
        [PrimaryKey, Identity]
        public int ID { get; set; }
        [Column(Name="Name"), NotNull]
        public string Name { get; set; }
    }
}
