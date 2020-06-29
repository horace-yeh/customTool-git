using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public class AdoTool
    {
        public string GetDbConnection(string connName)
        {
            var connection = ConfigurationManager.ConnectionStrings[connName].ToString();
            return connection;
        }

        #region 輸入參數SqlParameter

        public int ExecueNonQuery(string connStr, string sql, SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text; //設定為SQL語法;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable ExecuteDataTable(string connStr, string sql, SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
                {
                    adapter.SelectCommand.CommandType = CommandType.Text; //設定為SQL語法
                    if (pms != null)
                    {
                        adapter.SelectCommand.Parameters.AddRange(pms);
                    }
                    adapter.Fill(dt);
                }
                Sql_Conn.Close();
            }
            return dt;
        }

        #endregion 輸入參數SqlParameter

        #region 輸入參數Dictionary

        public int ExecueNonQuery(string connStr, string sql, Dictionary<string, object> pms)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text; //設定為SQL語法;
                    if (pms != null)
                    {
                        var pmsArr = this.DictionaryToSqlParameter(pms);
                        cmd.Parameters.AddRange(pmsArr);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable ExecuteDataTable(string connStr, string sql, Dictionary<string, object> pms)
        {
            DataTable dt = new DataTable();
            using (SqlConnection Sql_Conn = new SqlConnection(connStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
                {
                    adapter.SelectCommand.CommandType = CommandType.Text; //設定為SQL語法
                    if (pms != null)
                    {
                        var pmsArr = this.DictionaryToSqlParameter(pms);
                        adapter.SelectCommand.Parameters.AddRange(pmsArr);
                    }
                    adapter.Fill(dt);
                }
                Sql_Conn.Close();
            }
            return dt;
        }

        #endregion 輸入參數Dictionary

        /// <summary>
        /// Dictionary To SqlParamete Array
        /// </summary>
        /// <param name="pmsData"></param>
        /// <returns></returns>
        private SqlParameter[] DictionaryToSqlParameter(Dictionary<string, object> pmsData)
        {
            List<SqlParameter> p = new List<SqlParameter>();
            foreach (var item in pmsData)
            {
                p.Add(new SqlParameter(item.Key, item.Value));
            }
            return p.ToArray<SqlParameter>();
        }
    }
}