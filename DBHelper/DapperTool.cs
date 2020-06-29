using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public class DapperTool
    {
        #region Example

        //private List<T> GetData<T>(string sql, string StartDate, string EndDate)
        //{
        //    var DPTool = new DapperTool();
        //    string connName = "TBS2ConnectionString";
        //    // 宣告輸入參數型態
        //    var pms = new { StartDate = sTool.ToVarchar(StartDate, 23), EndDate = sTool.ToVarchar(EndDate, 23) };
        //    var data = DPTool.DapperQuery<T>(sTool.GetDbConnection(connName), sql, pms);
        //    return data;
        //}

        #endregion Example

        public int DapperNonQuery(string connStr, string sql, object pms)
        {
            int effectCounter = 0;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                effectCounter = Sql_Conn.Execute(sql, pms);
            }
            return effectCounter;
        }

        public int DapperNonQuerySP(string connStr, string sql, object pms)
        {
            int effectCounter = 0;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                effectCounter = Sql_Conn.Execute(sql, pms, commandType: CommandType.StoredProcedure);
            }
            return effectCounter;
        }

        public List<T> DapperQuery<T>(string connStr, string sql, object pms, int? timeout = null)
        {
            List<T> data = null;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                data = Sql_Conn.Query<T>(sql, pms, commandTimeout: timeout).ToList();
            }
            return data;
        }

        public List<T> DapperQuerySP<T>(string connStr, string sql, object pms, int? timeout = null)
        {
            List<T> data = null;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                data = Sql_Conn.Query<T>(sql, pms, commandType: CommandType.StoredProcedure, commandTimeout: timeout).ToList();
            }
            return data;
        }

        #region 設定Dapper參數

        //參數型態的定義可以有效提升效能
        //Ref: https://dotblogs.com.tw/supershowwei/2019/08/12/232213

        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public DbString ToChar(string me)
        {
            return new DbString { Value = me, IsAnsi = true, IsFixedLength = true };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public DbString ToChar(string me, int length)
        {
            return new DbString { Value = me, Length = length, IsAnsi = true, IsFixedLength = true };
        }

        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public DbString ToNChar(string me)
        {
            return new DbString { Value = me, IsFixedLength = true };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public DbString ToNChar(string me, int length)
        {
            return new DbString { Value = me, Length = length, IsFixedLength = true };
        }

        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public DbString ToNVarchar(string me)
        {
            return new DbString { Value = me };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public DbString ToNVarchar(string me, int length)
        {
            return new DbString { Value = me, Length = length };
        }

        /// <summary>
        ///     Length of the string is default 4000
        /// </summary>
        public DbString ToVarchar(string me)
        {
            return new DbString { Value = me, IsAnsi = true };
        }

        /// <summary>
        ///     Length of the string -1 for max
        /// </summary>
        public DbString ToVarchar(string me, int length)
        {
            return new DbString { Value = me, Length = length, IsAnsi = true };
        }

        #endregion 設定Dapper參數
    }
}