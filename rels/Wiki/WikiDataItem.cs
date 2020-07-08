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
        public Dictionary<string, string> Labels { get; internal set; }
        public Dictionary<string, string> Descriptions { get; internal set; }
    }

}
