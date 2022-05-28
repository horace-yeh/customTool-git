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
    public class ConnectionFactory
    {
        public IDbConnection CreateConnection(string connName = "MainConnectionString")
        {
            switch (connName)
            {
                case "MainConnectionString":
                    var conn = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
                    return new SqlConnection(conn);
                default:
                    throw new ArgumentException("Not Found ConnectionName");
            }
        }
    }
}
