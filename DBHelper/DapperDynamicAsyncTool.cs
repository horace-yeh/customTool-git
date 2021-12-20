using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public class DapperDynamicAsyncTool
    {
        #region Example
        //DapperDynamicAsyncTool dapperTool = new DapperDynamicAsyncTool("MainConnectionString");;
        //var sql = @"select * from [Sales].[Store] Where BusinessEntityID = @BusinessEntityID And Name = @Name";
        //var pms = new Dapper.DynamicParameters();
        //pms.Add("Name", "Next-Door Bike Store", DbType.String, size: 50);
        //pms.Add("BusinessEntityID", 292, DbType.Int32);
        //var data = await this.dapperTool.DapperQueryAsync<Store>(sql, pms);

        #endregion Example


        private readonly string ConnString;           //資料庫連線參數
        public DapperDynamicAsyncTool(string connectionName)
        {
            this.ConnString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public async Task<int> DapperNonQueryAsync(string sql, SqlMapper.IDynamicParameters pms)
        {
            int effectCounter = 0;
            using (SqlConnection sqlConn = new SqlConnection(this.ConnString))
            {
                effectCounter = await sqlConn.ExecuteAsync(sql, pms);
            }
            return effectCounter;
        }

        public async Task<int> DapperNonQuerySpAsync(string sql, SqlMapper.IDynamicParameters pms)
        {
            int effectCounter = 0;
            using (SqlConnection sqlConn = new SqlConnection(this.ConnString))
            {
                effectCounter = await sqlConn.ExecuteAsync(sql, pms, commandType: CommandType.StoredProcedure);
            }
            return effectCounter;
        }

        public async Task<List<T>> DapperQueryAsync<T>(string sql, SqlMapper.IDynamicParameters pms, int? timeout = null)
        {
            List<T> data = null;
            using (SqlConnection sqlConn = new SqlConnection(this.ConnString))
            {
                var temp = await sqlConn.QueryAsync<T>(sql, pms, commandTimeout: timeout);
                data = temp.ToList();
            }
            return data;
        }

        public async Task<List<T>> DapperQuerySpAsync<T>(string sql, SqlMapper.IDynamicParameters pms, int? timeout = null)
        {
            List<T> data = null;
            using (SqlConnection sqlConn = new SqlConnection(this.ConnString))
            {
                var temp = await sqlConn.QueryAsync<T>(sql, pms, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
                data = temp.ToList();
            }
            return data;
        }


        #region DbType Description

        private void Desc()
        {
            var typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
        }

        #endregion DbType Description
    }
}
