using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;

namespace HelperTools
{
    public class MySqlDataHandle
    {
        public int Ipage = 20;
        public string DataConnType = "mysql";
        IDal dal;
        public string ThisConn;
        public string _CurrentToken;
        /// <summary>
        /// 类型转换
        /// </summary>
        public string ConvertDataType(string ColType)
        {
            string data_type = "N";
            switch (ColType)
            {
                case "int":
                    data_type = "I";
                    break;
                case "bigint":
                    data_type = "I";
                    break;
                case "smallint":
                    data_type = "I";
                    break;
                case "decimal":
                    data_type = "F";
                    break;
                case "float":
                    data_type = "F";
                    break;
                case "numeric":
                    data_type = "F";
                    break;
                case "real":
                    data_type = "F";
                    break;
                case "money":
                    data_type = "F";
                    break;
                case "nchar":
                    data_type = "N";
                    break;
                case "nvarchar":
                    data_type = "N";
                    break;
                case "varchar":
                    data_type = "N";
                    break;
                case "char":
                    data_type = "N";
                    break;
                case "text":
                    data_type = "N";
                    break;
                case "ntext":
                    data_type = "N";
                    break;
                case "datetime":
                    data_type = "D";
                    break;
                case "date":
                    data_type = "D";
                    break;
                case "smalldatetime":
                    data_type = "D";
                    break;
                default:
                    throw new Exception("Model生成器未实现数据库字段类型的转换");
            }
            return data_type;
        }
        
        /// <summary>
        /// MySQL值 类型转换
        /// </summary>
        /// <param name="ColType"></param>
        /// <returns></returns>
        public MySqlDbType ConvertMySQLType(string ColType)
        {
            MySqlDbType data_type = MySqlDbType.String;
            switch (ColType)
            {
                case "int":
                    data_type = MySqlDbType.Int32;
                    break;
                case "bigint":
                    data_type = MySqlDbType.Int64;
                    break;
                case "smallint":
                    data_type = MySqlDbType.Int16;
                    break;
                case "decimal":
                    data_type = MySqlDbType.Decimal;
                    break;
                case "float":
                    data_type = MySqlDbType.Float;
                    break;
                case "numeric":
                    data_type = MySqlDbType.UInt64;
                    break;
                case "bool":
                    data_type = MySqlDbType.Bit;
                    break;
                case "double":
                    data_type = MySqlDbType.Double;
                    break;
                case "nchar":
                    data_type = MySqlDbType.String; 
                    break;
                case "nvarchar":
                    data_type = MySqlDbType.VarChar;
                    break;
                case "varchar":
                    data_type = MySqlDbType.VarChar;
                    break;
                case "char":
                    data_type = MySqlDbType.String;
                    break;             
                case "timestamp":
                    data_type = MySqlDbType.Timestamp;
                    break;
                case "datetime":
                    data_type = MySqlDbType.DateTime;
                    break;
                case "date":
                    data_type = MySqlDbType.DateTime;
                    break;
                default:
                    data_type = MySqlDbType.String;
                    break;
                    //throw new Exception("Model生成器未实现数据库字段类型的转换");
            }
            return data_type;
        }
        /// <summary>
        /// 清理过程变量
        /// </summary>
        /// <param name="Work"></param>
        /// <returns></returns>
        public WorkResult ClearResult(WorkResult Work, bool Flg)
        {
            WorkResult Result = new WorkResult();
            Result = Work;
            Result.SqlCmd = string.Empty;
            Result.Fields = string.Empty;
            Result.Values = string.Empty;
            Result.SqlParas = new SqlPara();
            Result.EnityList = new SqlEnity();
            Result.ID = string.Empty;
            Result.Up = string.Empty;
            Result.RedisKey = string.Empty;
            Result.Result = string.Empty;
            Result.Json = string.Empty;
            Result.ListFields = string.Empty;
            Result.ListFieldsCn = string.Empty;
            if (Flg)
            {
                Result.Code = string.Empty;
                Result.UnitCode = string.Empty;
            }

            return Result;
        }
        /// <summary>
        /// 列表对象，需要返回datatable 和列表字段配置
        /// </summary>
        public class ListObject
        {
            public DataTable ListDt = new DataTable();
            public string ListCfg { get; set; }
        }
        /// <summary>
        /// 分页参数
        /// </summary>
        public class Paging
        {
            /*首页*/
            public long Spage { get; set; }
            /*下一页*/
            public long NextPage { get; set; }
            /*上页*/
            public long PrePage { get; set; }
            /*末页*/
            public long EndPage { get; set; }
            /*总页数*/
            public long Rpage { get; set; }
            /*总记录数*/
            public long Rnum { get; set; }
            /*当前页*/
            public long CurrentPage { get; set; }
            public long MaxNum { get; set; }
            public long MinNum { get; set; }
            public string PagingMode { get; set; }//翻页模式 S-首面 N-下一页 P-上一页 E--末页
            public long LastID { get; set; }
        }
        /// <summary>
        /// 过程操作变量集合
        /// </summary>
        public class WorkResult
        {
            public string Result { get; set; }
            public string SqlCmd { get; set; }
            public List<SqlParaResult> HsTb = new List<SqlParaResult>();
            public SqlEnity EnityList = new SqlEnity();
            public SqlPara SqlParas = new SqlPara();
            public string Fields { get; set; }
            public string Values { get; set; }
            public string Up { get; set; }
            public string ID { get; set; }
            public string Code { get; set; }
            public string UnitCode { get; set; }
            public string RedisKey { get; set; }
            public string Json { get; set; }
            public string ListFields { get; set; }
            public string ListFieldsCn { get; set; }
        }


  
        /// <summary>
        /// 多表多记录参数集合
        /// </summary>
        public class SqlParaResult
        {
            public string SqlCmd { get; set; }
            public SqlEnity ValuePara = new SqlEnity();
        }
        /// <summary>
        /// 多表多记录参数集合
        /// </summary>
        public class SqlEnity
        {
            public List<SqlPara> ValuePara = new List<SqlPara>();
        }
        /// <summary>
        /// 单表参数/单记录参数
        /// </summary>
        public class SqlPara
        {
            public List<SQLtype> ValuePara = new List<SQLtype>();
        }
        /// <summary>
        /// 值参数属性
        /// </summary>
        public  class SQLtype
        {
            public string ColName { get; set; }
            public MySqlDbType ColType { get; set; }
            public int ColLeng { get; set; }
            public string ColValue { get; set; }
        }
        /// <summary>
        /// 联表别名计算
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetTbAlias(int num)
        {
            string s = string.Empty;
            string ss = "1:A,2:B,3:C,4:D,5:E,6:F,7:G,8:H,9:I,10:J,11:K,12:L,13:M,14:O";
            string[] arr = ss.Split(',');
            for (int x = 0; x < arr.Length; x++)
            {
                string str = arr[x];
                string[] strarr = str.Split(':');
                if (strarr[0] == num.ToString())
                {
                    s = strarr[1];
                    break;
                }
            }
            return s;
        }
        /// <summary>
        /// 公共返回JsonResult 封装
        /// </summary>
        /// <param name="scuess">1执行成功 0执行失败</param>
        /// <param name="Msg">返回信息</param>
        /// <param name="IsResult">1有返回数据 0无返回数据</param>
        /// <param name="Dt">返回的数据</param>
        /// <param name="IsToken"></param>
        /// <returns></returns>
        public string GetResult(bool scuess, string Msg, bool IsResult, object Dt,bool IsToken=false)
        {
            string Result = string.Empty;
            StringBuilder sb = new StringBuilder();
            Dictionary<string, dynamic> s = new Dictionary<string, dynamic>
            {
                {"CurrentTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") },
                {"scuess" ,scuess},
                {"Msg" ,Msg},
                {"IsResult" ,IsResult},
                {"Result" ,Dt},
                { "IsToken",IsToken}
            };
            Result = JsonConvert.SerializeObject(s);
            return Result;
        }

        public string PackUserField(string Fields, bool UnitFlg)
        {
            string Result = string.Empty;
            if (UnitFlg == true)
            {
                Result = Fields + ",Author,Manager";
            }
            else
            {
                Result = Fields + ",Author,Manager,UnitCode";
            }
            return Result;
        }
        public string PackUserValues(string Values, bool UnitFlg)
        {
            string Result = string.Empty;
            if (UnitFlg == true)
            {
                Result = Values + ",@Author,@Manager";
            }
            else
            {
                Result = Values + ",@Author,@Manager,@UnitCode";
            }
            return Result;
        }
        public SqlPara PackUserParas(SqlPara SqlParas, JObject UserKey,bool UnitFlg)
        {
            SqlPara Result = new SqlPara ();
            Result = SqlParas;
            Result.ValuePara.Add(new SQLtype {
                ColName = "@Author",
                ColType = ConvertMySQLType("nvarchar"),
                ColLeng = 50,
                ColValue = UserKey["User"].ToString()
            });
            Result.ValuePara.Add(new SQLtype
            {
                ColName = "@Manager",
                ColType = ConvertMySQLType("nvarchar"),
                ColLeng = 50,
                ColValue = UserKey["User"].ToString()
            });
            if (!UnitFlg )
            {
                Result.ValuePara.Add(new SQLtype
                {
                    ColName = "@UnitCode",
                    ColType = ConvertMySQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = UserKey["UnitCode"].ToString()
                });
            }
            return Result;
        }
        public string PackUpInsField(string Fields)
        {
            string Result = string.Empty;
            Result = Fields + ",LastModiDate=@LastModiDate,Manager=@Manager ";
            return Result;
        }
        public SqlPara PackUpInsSqlParas(SqlPara SqlParas, JObject UserKey)
        {
            SqlPara Result = new SqlPara();
            Result = SqlParas;
            Result.ValuePara.Add(new SQLtype
            {
                ColName = "@LastModiDate",
                ColType = ConvertMySQLType("datetime"),
                ColLeng = 8,
                ColValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
            });
            Result.ValuePara.Add(new SQLtype
            {
                ColName = "@Manager",
                ColType = ConvertMySQLType("nvarchar"),
                ColLeng = 50,
                ColValue = UserKey["User"].ToString()
            });    
            return Result;
        }
        public bool JtokenFlg(JToken Exists)
        {
            bool Flg ;
            if (Exists == null)
            {
                Flg = false;
            }
            else
            {
                Flg = (Exists.Type == JTokenType.None || Exists.Type == JTokenType.Null)
                    ? false : true;
            }
            return Flg;
        }
        public Paging InitPageStart(JObject PagePara)
        {
            long StartPapge = PagePara["StartPapge"].ToString() == "0"
                ? 1
                : (long)PagePara["StartPapge"];//起始页
            Paging page = new Paging()
            {
                Rnum = PagePara["Rnum"].ToString() == "0"
                    ? 0
                    : (long)PagePara["Rnum"],
                Rpage = PagePara["Rpage"].ToString() == "0"
                    ? 0
                    : (long)PagePara["Rpage"],
                MinNum = PagePara["MinNum"].ToString() == "0"
                    ? 0
                    : (long)PagePara["MinNum"],
                MaxNum = PagePara["MaxNum"].ToString() == "0"
                    ? 0
                    : (long)PagePara["MaxNum"],
                LastID = PagePara["LastID"].ToString() == "0"
                    ? 0
                    : (long)PagePara["LastID"],
                CurrentPage = StartPapge ,
                PagingMode = PagePara["PagingMode"].ToString()
            };

            return page;
        }
    }
}
