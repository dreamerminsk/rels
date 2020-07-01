using LinqToDB.Mapping;
using System.Collections.Generic;

namespace rels.Model
{
    [Table(Name = "People")]
    public class Person
    {

        [PrimaryKey, Identity]
        public int ID { get; set; }

        [Column(Name = "WikiDataID")]
        public string WikiDataID { get; set; }

        [Column(Name = "ImageFile")]
        public string ImageFile { get; set; }

        [Association(
            ThisKey = nameof(WikiDataID),
            OtherKey = nameof(Label.WikiDataID))]
        public List<Label> Labels { get; set; } = new List<Label>();

        [Column(Name = "Description")]
        public string Description { get; set; }

        [Column(Name = "Country")]
        public string Country { get; set; }

        [Column(Name = "DateOfBirth")]
        public string DateOfBirth { get; set; }

        [Column(Name = "DateOfDeath")]
        public string DateOfDeath { get; set; }

        [Column(Name = "Father")]
        public string Father { get; set; }

        [Column(Name = "Mother")]
        public string Mother { get; set; }

        public List<string> Siblings { get; set; } = new List<string>();

        public List<string> Spouse { get; set; } = new List<string>();

        public List<string> Children { get; set; } = new List<string>();
    }
}
