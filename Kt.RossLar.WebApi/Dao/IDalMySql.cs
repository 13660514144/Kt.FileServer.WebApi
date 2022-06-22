using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace HelperTools
{
    public interface IDalMySql
    {
        DataTable MySqlListData(string SQLstring);
        long ExecuteMySql(string SQLstring);
        DataTable ListData(string SQLstring, MySqlDataHandle.SqlPara SqlPara);
        long ExeScalar(string SQLstring, MySqlDataHandle.SqlPara SqlPara);
        int UpOrIns(string SQLstring, MySqlDataHandle.SqlPara SqlPara);
        bool UpOrIns(List<MySqlDataHandle.SqlParaResult> SqlPara);

    }
}
