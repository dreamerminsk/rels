using LinqToDB.Data;
using System;
using System.Windows.Forms;

namespace rels.Model
{
    public partial class RelsDB : LinqToDB.Data.DataConnection
    {

        private static bool isInit = false;

        partial void InitDataContext()
        {
            if (!isInit)
            {
                isInit = true;
                MessageBox.Show("INIT " + DateTime.Now);
                Init.CREATE_SQL.ForEach(async sql => await this.ExecuteAsync(sql));
            }
        }

    }
}
