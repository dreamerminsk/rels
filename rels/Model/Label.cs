﻿using LinqToDB.Mapping;

namespace rels.Model
{
    [Table(Name = "Labels")]
    public class Label
    {
        [PrimaryKey, Identity]
        public int ID { get; set; }

        [Column(Name = "WikiDataID")]
        public string WikiDataID { get; set; }

        [Column(Name = "Language")]
        public string Language { get; set; }

        [Column(Name = "Value")]
        public string Value { get; set; }
    }
}
