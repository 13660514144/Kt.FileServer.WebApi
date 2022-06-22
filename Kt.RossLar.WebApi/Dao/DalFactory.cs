using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTools
{
    /// <summary>
    /// 数据库操作工厂类
    /// </summary>
    public class DalFactory
    {
        /// <summary>
        /// 创建Dal
        /// </summary>
        /// <param name="databaseType">数据库类型，如SQLite、MySql</param>
        public static IDal CreateDal(string databaseType,string CONN)
        {
            switch (databaseType.ToLower())
            {
                //case "mysql":
                //    MySqlDal MysqlDal = new MySqlDal();
                //    MysqlDal.CONN = CONN;
                //    return MysqlDal;
                //    return new SQLiteDal();
                //case "sqlite":
                //    return new SQLiteDal();
                //case "oracle":
                //    return new OracleDal();
                case "mssql":
                    MSSQLDal MsDal = new MSSQLDal();
                    MsDal.CONN = CONN;
                    return MsDal;
                default:
                    throw new Exception("数据库类型错误");
            }
        }
        /// <summary>
        /// 创建MYSQLdao
        /// </summary>
        /// <param name="databaseType"></param>
        /// <param name="CONN"></param>
        /// <returns></returns>
        public static IDalMySql CreateMySqlDal(string databaseType, string CONN)
        {
            switch (databaseType.ToLower())
            {
                case "mysql":
                    MySqlDal MysqlDal = new MySqlDal();
                    MysqlDal.CONN = CONN;
                    return MysqlDal;
                //    return new SQLiteDal();
                //case "sqlite":
                //    return new SQLiteDal();
                //case "oracle":
                //    return new OracleDal();
                
                default:
                    throw new Exception("数据库类型错误");
            }

        }
    }
}
