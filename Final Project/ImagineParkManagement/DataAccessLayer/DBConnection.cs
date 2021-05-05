using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal static class DBConnection
    {
        // this is the only place to connect to the db
        private static String connectionString =
            @"Data Source=(local)\SQLEXPRESS;Initial Catalog=imagine_park_db;Integrated Security=True";
        // add home string in comment file here

        public static SqlConnection GetDBConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
