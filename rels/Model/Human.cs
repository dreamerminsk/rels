using LinqToDB.Mapping;
using System;
using System.Collections.Generic;

namespace rels.Model
{
    [Table(Name = "Humans")]
    public class Human
    {

        [PrimaryKey, Identity]
        public int ID { get; set; }

        [Column(Name = "WikiDataID")]
        public string WikiDataID { get; set; }

        public string Instance { get; set; }

        [Column(Name = "ImageFile")]
        public string ImageFile { get; set; }

        [Association(
            ThisKey = nameof(WikiDataID),
            OtherKey = nameof(Label.WikiDataID))]
        public List<Label> Labels { get; set; } = new List<Label>();

        [Association(
            ThisKey = nameof(WikiDataID),
            OtherKey = nameof(Description.WikiDataID))]
        public List<Description> Descriptions { get; set; } = new List<Description>();

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

        [Column(Name = "Modified")]
        public DateTime Modified { get; set; }

    }
}
