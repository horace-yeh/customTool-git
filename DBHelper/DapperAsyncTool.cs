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
    public class DapperAsyncTool
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

        public async Task<int> DapperNonQueryAsync(string connStr, string sql, object pms)
        {
            int effectCounter = 0;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                effectCounter = await Sql_Conn.ExecuteAsync(sql, pms);
            }
            return effectCounter;
        }

        public async Task<int> DapperNonQuerySPAsync(string connStr, string sql, object pms)
        {
            int effectCounter = 0;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                effectCounter = await Sql_Conn.ExecuteAsync(sql, pms, commandType: CommandType.StoredProcedure);
            }
            return effectCounter;
        }

        public async Task<List<T>> DapperQueryAsync<T>(string connStr, string sql, object pms, int? timeout = null)
        {
            List<T> data = null;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                var temp = await Sql_Conn.QueryAsync<T>(sql, pms, commandTimeout: timeout);
                data = temp.ToList();
            }
            return data;
        }

        public async Task<List<T>> DapperQuerySPAsync<T>(string connStr, string sql, object pms, int? timeout = null)
        {
            List<T> data = null;
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                var temp = await Sql_Conn.QueryAsync<T>(sql, pms, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
                data = temp.ToList();
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
