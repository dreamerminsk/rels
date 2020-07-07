using rels.Model;
using System;
using System.Collections.Generic;

namespace rels.Wiki
{

    public class WikiDataItem
    {

        public WikiDataItem() { }

        public string WikiDataID { get; internal set; }
        public int ID { get; internal set; }
        public DateTime Modified { get; internal set; }
        public List<Label> Labels { get; internal set; }
    }

}
