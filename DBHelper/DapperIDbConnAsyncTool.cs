using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DBHelper
{
    public class DapperIDbConnAsyncTool
    {
        private readonly ConnectionFactory _connectionFactory;
        public DapperIDbConnAsyncTool()
        {
            this._connectionFactory = new ConnectionFactory();
        }

        public async Task<int> DapperNonQueryAsync(string sql, object pms)
        {
            using (var conn = this._connectionFactory.CreateConnection())
            {
                return await conn.ExecuteAsync(sql, pms);
            }
        }

        public async Task<int> DapperNonQuerySPAsync(string sql, object pms)
        {
            using (var conn = this._connectionFactory.CreateConnection())
            {
                return await conn.ExecuteAsync(sql, pms, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> DapperQueryAsync<T>(string sql, object pms, int? timeout = null)
        {

            using (var conn = this._connectionFactory.CreateConnection())
            {
                return await conn.QueryAsync<T>(sql, pms, commandTimeout: timeout);
            }
        }

        public async Task<IEnumerable<T>> DapperQuerySPAsync<T>(string sql, object pms, int? timeout = null)
        {
            using (var conn = this._connectionFactory.CreateConnection())
            {
                return await conn.QueryAsync<T>(sql, pms, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            }
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
