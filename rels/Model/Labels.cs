﻿using LinqToDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Linq;

namespace rels.Model
{
    public class Labels
    {

        public static List<Label> GetLabels(string wikiDataID)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    var labels = db.GetTable<Label>();
                    return labels.Where(l => l.WikiDataID.Equals(wikiDataID)).ToList();
                }
                catch (Exception e)
                {
                    return new List<Label>();
                }
            }
        }

        public static int Insert(Label label)
        {
            using (var db = new RelsDB())
            {
                try
                {
                    return db.Insert(label);
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }
    }
}
