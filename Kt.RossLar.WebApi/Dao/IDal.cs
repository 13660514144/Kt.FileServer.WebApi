using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTools
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    public interface IDal
    {
        /// <summary>
        /// 获取数据库名
        /// </summary>
        List<Dictionary<string, string>> GetAllTables();
        /// <summary>
        /// 获取表的所有字段名及字段类型
        /// </summary>
        List<Dictionary<string, string>> GetAllColumns(string tableName);
        /// <summary>
        /// 类型转换
        /// </summary>
        string ConvertDataType(Dictionary<string, string> column);
        DataTable ListData(string SQLstring);
        DataTable ListData(string SQLstring, DataHandle.SqlPara SqlPara );
        DataTable GetDataBase();
        bool UpOrInsQuery(string SQLstring);
        int UpOrIns(string SQLstring, DataHandle.SqlPara SqlPara);

        bool UpOrIns(List<DataHandle.SqlParaResult> SqlPara);
        int UpOrIns(string SQLstring);
        long ExeScalar(string SQLstring, DataHandle.SqlPara SqlPara);
        ///////////////
        
    }
}
