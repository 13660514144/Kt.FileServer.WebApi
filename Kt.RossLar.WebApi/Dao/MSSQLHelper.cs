using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace HelperTools
{
    /// <summary>
    /// 操作类
    /// </summary>
    public class MSSQLHelper
    {
        int TimeOut = 300;
        #region 变量
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string connectionString = "";// ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
        //private string connectionString = GetCONN();
        #endregion

        #region Exists
        public bool ExeScalar(string SQLString)
        {
            bool IfOk= true;

            SqlTransaction trans = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    cmd.CommandTimeout = TimeOut;
                    try
                    {                        
                        connection.Open();
                        trans = connection.BeginTransaction();
                        cmd.Transaction = trans;
                        object obj = cmd.ExecuteScalar();
                        trans.Commit();                       
                    }
                    catch (Exception ex)
                    {
                        IfOk = false;
                        trans.Rollback();
                        connection.Close();
                        LogHelper.ErrorLog(ex);
                        //throw new Exception(ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            return IfOk;
        }
        public long ExeScalar(string SQLString, DataHandle.SqlPara SqlPara)
        {
            long Rows = 0;
            //LogHelper.Error(SQLString);
            SqlTransaction trans = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    cmd.CommandTimeout = TimeOut;
                    try
                    {
                        foreach (DataHandle.SQLtype item in SqlPara.ValuePara)
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
                        connection.Close();
                                                
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
        #endregion

        #region 执行SQL语句，返回影响的记录数
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString)
        {
            SqlTransaction trans = null;
            int rows = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    cmd.CommandTimeout = TimeOut;
                    try
                    {
                        connection.Open();
                        trans = connection.BeginTransaction();
                        cmd.Transaction = trans;
                        rows = cmd.ExecuteNonQuery();
                        trans.Commit();                       
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        connection.Close();
                        LogHelper.ErrorLog(ex);
                        //throw new Exception(ex.Message);
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
        public int ExecuteSql(string SQLString, DataHandle.SqlPara SqlPara)
        {
            SqlTransaction trans = null;
            int rows = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    cmd.CommandTimeout = TimeOut;
                    try
                    {
                        foreach (DataHandle.SQLtype item in SqlPara.ValuePara)
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
                        connection.Close();
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

        public bool ExecuteSql(List<DataHandle.SqlParaResult> SqlPara)
        {
            bool Flg = false;            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //声明事务
                SqlTransaction tr = conn.BeginTransaction();
                SqlCommand comm = new SqlCommand();
                comm.CommandTimeout = TimeOut;
                comm.Connection = conn;
                //指定给SqlCommand事务
                comm.Transaction = tr;
                try
                {
                    //遍历Hashtable数据，每次遍历执行SqlCommand
                    foreach (DataHandle.SqlParaResult de in SqlPara)
                    {
                        string cmdText = de.SqlCmd.ToString();
                        DataHandle.SqlEnity Collection = de.ValuePara;
                        foreach (DataHandle.SqlPara list in Collection.ValuePara)
                        {
                            //指定执行语句
                            comm.CommandText = cmdText;
                            foreach (DataHandle.SQLtype item in list.ValuePara)
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

        #endregion

        #region 执行查询语句，返回DataTable  字符串拼接模式
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string SQLString)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = TimeOut;
                    cmd.CommandText = SQLString;

                    cmd.Connection = connection;

                    SqlDataReader read = cmd.ExecuteReader();
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
        #endregion
        #region 执行查询语句，返回DataTable  参数模式
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable Query(string SQLString, DataHandle.SqlPara SqlPara)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                   
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = TimeOut;
                    cmd.CommandText = SQLString;

                    foreach (DataHandle.SQLtype item in SqlPara.ValuePara)
                    {
                        cmd.Parameters.Add(item.ColName, item.ColType, item.ColLeng).Value = item.ColValue;
                    }

                    cmd.Connection = connection;

                    SqlDataReader read = cmd.ExecuteReader();
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
        #endregion
        /// <summary>
        /// SqlDataReader 转成 DataTable
        /// 源需要是结果集
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public  DataTable DataReaderToDataTable(SqlDataReader dataReader)
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
                        myDataRow[i] =(dataReader[i] is DBNull) ? string.Empty : dataReader[i].ToString();
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
