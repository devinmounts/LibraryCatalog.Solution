using System;
using MySql.Data.MySqlClient;
using LibraryCatalog;
namespace LibraryCatalog.Models
{
    public class DB
    {
        public static MySqlConnection Connection()
        {
            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
}
