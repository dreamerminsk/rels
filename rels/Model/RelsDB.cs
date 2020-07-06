using LinqToDB.Data;

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
                Init.CREATE_SQL.ForEach(async sql => await this.ExecuteAsync(sql));
            }
        }

    }
}
