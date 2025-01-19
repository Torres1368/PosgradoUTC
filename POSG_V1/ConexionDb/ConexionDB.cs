using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace POSG_V1.ConexionDb
{
    public class ConexionDB
    {
        private readonly string connectionString;

        public ConexionDB()
        {
            connectionString = "data source=.; database=TESIS; integrated security=SSPI";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}