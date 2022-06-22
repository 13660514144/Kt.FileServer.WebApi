using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace HelperTools
{
    public class MySQLHelper
    {
        private ILogger _logger;
        public MySQLHelper()
        {
            //_logger = ContainerHelper.Resolve<ILogger>();
            //_logger = logger;
        }
        int TimeOut = 300;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string connectionString = "";
        

        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string SQLString)
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();               
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);                    
                    command.Fill(ds);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    LogHelper.InfoLog($"ExecuteDataTable err :{ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
                return ds.Tables[0];

            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public long ExecuteMySql(string SQLString)
        {
            long rows = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        rows = cmd.ExecuteNonQuery();                        
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        LogHelper.InfoLog($"ExecuteDataTable err :{e.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return rows;
        }
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string SQLString, MySqlDataHandle.SqlPara SqlPara)
        {
            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandTimeout = TimeOut;
                    cmd.CommandText = SQLString;

                    foreach (MySqlDataHandle.SQLtype item in SqlPara.ValuePara)
                    {
                        cmd.Parameters.Add(item.ColName, item.ColType, item.ColLeng).Value = item.ColValue;
                    }

                    cmd.Connection = connection;

                    MySqlDataReader read = cmd.ExecuteReader();
                    table = DataReaderToDataTable(read);
                }
                catch (Exception ex)
                {
                    LogHelper.ErrorLog(ex);
                    LogHelper.ErrorLog(SQLString);
                    //throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return table;
        }
        public long ExeScalar(string SQLString, MySqlDataHandle.SqlPara SqlPara)
        {
            long Rows = 0;
            //LogHelper.Error(SQLString);
            MySqlTransaction trans = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    cmd.CommandTimeout = TimeOut;
                    try
                    {
                        foreach (MySqlDataHandle.SQLtype item in SqlPara.ValuePara)
                        {
                            cmd.Parameters.Add(item.ColName, item.ColType, item.ColLeng).Value = item.ColValue;
                        }
                        connection.Open();
                        trans = connection.BeginTransaction();
                        cmd.Transaction = trans;
                        Rows = Convert.ToInt64(cmd.ExecuteScalar());
                        trans.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogHelper.ErrorLog(ex);
                        trans.Rollback();                       
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            return Rows;
        }
        public int ExecuteSql(string SQLString, MySqlDataHandle.SqlPara SqlPara)
        {
            MySqlTransaction trans = null;
            int rows = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    cmd.CommandTimeout = TimeOut;
                    try
                    {
                        foreach (MySqlDataHandle.SQLtype item in SqlPara.ValuePara)
                        {
                            cmd.Parameters.Add(item.ColName, item.ColType, item.ColLeng).Value = item.ColValue;
                        }
                        connection.Open();
                        trans = connection.BeginTransaction();
                        cmd.Transaction = trans;
                        rows = cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        LogHelper.ErrorLog(ex);

                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            return rows;
        }
        public bool ExecuteSql(List<MySqlDataHandle.SqlParaResult> SqlPara)
        {
            bool Flg = false;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                //声明事务
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand comm = new MySqlCommand();
                comm.CommandTimeout = TimeOut;
                comm.Connection = conn;
                //指定给SqlCommand事务
                comm.Transaction = tr;
                try
                {
                    //遍历Hashtable数据，每次遍历执行SqlCommand
                    foreach (MySqlDataHandle.SqlParaResult de in SqlPara)
                    {
                        string cmdText = de.SqlCmd.ToString();
                        MySqlDataHandle.SqlEnity Collection = de.ValuePara;
                        foreach (MySqlDataHandle.SqlPara list in Collection.ValuePara)
                        {
                            //指定执行语句
                            comm.CommandText = cmdText;
                            foreach (MySqlDataHandle.SQLtype item in list.ValuePara)
                            {
                                comm.Parameters.Add(item.ColName, item.ColType, item.ColLeng).Value = item.ColValue;
                            }
                            //执行
                            comm.ExecuteNonQuery();
                            //使用后清空参数，为下次使用
                            comm.Parameters.Clear();
                        }
                    }
                    //不出意外事务提前，返回True
                    tr.Commit();
                    Flg = true;
                }
                catch (Exception ex)
                {
                    //出意外事务回滚，返回Fasle
                    tr.Rollback();
                    LogHelper.ErrorLog(ex);
                }
                finally
                {
                    comm.Dispose();
                    conn.Close();
                }
            }
            return Flg;
        }
        /// <summary>
        /// SqlDataReader 转成 DataTable
        /// 源需要是结果集
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public DataTable DataReaderToDataTable(MySqlDataReader dataReader)
        {
            //定义DataTable  
            DataTable datatable = new DataTable();

            try
            {    //动态添加表的数据列  
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    DataColumn myDataColumn = new DataColumn();
                    myDataColumn.DataType = dataReader.GetFieldType(i);
                    myDataColumn.ColumnName = dataReader.GetName(i);
                    datatable.Columns.Add(myDataColumn);
                }

                //添加表的数据  
                while (dataReader.Read())
                {
                    DataRow myDataRow = datatable.NewRow();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        myDataRow[i] = (dataReader[i] is DBNull) ? string.Empty : dataReader[i].ToString();
                    }
                    datatable.Rows.Add(myDataRow);
                    myDataRow = null;
                }
                //关闭数据读取器  
                dataReader.Close();
            }
            catch (Exception ex)
            {
                //抛出类型转换错误  
                LogHelper.ErrorLog(ex);
            }
            return datatable;
        }
    }
}
